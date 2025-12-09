import { ref, computed } from 'vue';
import type { Student, Statistics, DrawHistory } from '../types/student';
import * as lotteryApi from '../api/lottery';

/**
 * 抽签功能组合式函数
 */
export function useLottery() {
  // 状态
  const students = ref<Student[]>([]);
  const selectedStudent = ref<Student | null>(null);
  const selectedStudents = ref<Student[]>([]); // 批量抽签结果
  const statistics = ref<Statistics | null>(null);
  const classList = ref<string[]>([]);
  const isDrawing = ref(false);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // 筛选条件
  const filterGender = ref<string>('');
  const filterClass = ref<string>('');

  // 抽签历史
  const drawHistory = ref<DrawHistory[]>([]);
  const excludeDrawn = ref(false); // 是否排除已抽取的学生

  // 批量抽签设置
  const drawCount = ref(1); // 抽取数量

  // 计算属性
  const filteredStudents = computed(() => {
    let result = students.value;

    if (filterGender.value) {
      result = result.filter(s => s.gender === filterGender.value);
    }

    if (filterClass.value) {
      result = result.filter(s => s.class === filterClass.value);
    }

    // 排除已抽取的学生
    if (excludeDrawn.value && drawHistory.value.length > 0) {
      const drawnIds = new Set(drawHistory.value.map(h => h.studentId));
      result = result.filter(s => !drawnIds.has(s.id));
    }

    return result;
  });

  const filteredCount = computed(() => filteredStudents.value.length);

  /**
   * 加载所有学生
   */
  async function loadStudents() {
    try {
      isLoading.value = true;
      error.value = null;
      students.value = await lotteryApi.getAllStudents();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '加载学生列表失败';
      console.error('加载学生失败:', e);
    } finally {
      isLoading.value = false;
    }
  }

  /**
   * 加载统计信息
   */
  async function loadStatistics() {
    try {
      error.value = null;
      statistics.value = await lotteryApi.getStatistics();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '加载统计信息失败';
      console.error('加载统计失败:', e);
    }
  }

  /**
   * 加载班级列表
   */
  async function loadClassList() {
    try {
      error.value = null;
      classList.value = await lotteryApi.getClassList();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '加载班级列表失败';
      console.error('加载班级列表失败:', e);
    }
  }

  /**
   * 加载抽签历史
   */
  async function loadHistory() {
    try {
      error.value = null;
      const history = await lotteryApi.getDrawHistory();
      drawHistory.value = history;
    } catch (e) {
      error.value = e instanceof Error ? e.message : '加载历史记录失败';
      console.error('加载历史记录失败:', e);
    }
  }

  /**
   * 执行抽签
   */
  async function performDraw() {
    if (isDrawing.value) return;

    try {
      isDrawing.value = true;
      error.value = null;
      selectedStudent.value = null;
      selectedStudents.value = [];

      // 动画效果 - 快速切换显示
      const animationDuration = 2000; // 2秒动画
      const interval = 100; // 每100ms切换一次
      const iterations = animationDuration / interval;

      // 动画过程
      for (let i = 0; i < iterations; i++) {
        if (filteredStudents.value.length > 0) {
          const randomIndex = Math.floor(Math.random() * filteredStudents.value.length);
          const student = filteredStudents.value[randomIndex];
          if (student) {
            selectedStudent.value = student;
          }
          await new Promise(resolve => setTimeout(resolve, interval));
        }
      }

      // 真正的抽签
      const result = await lotteryApi.drawStudent(
        filterGender.value || undefined,
        filterClass.value || undefined
      );

      selectedStudent.value = result;

      // 重新加载历史记录
      await loadHistory();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '抽签失败';
      console.error('抽签失败:', e);
      selectedStudent.value = null;
    } finally {
      isDrawing.value = false;
    }
  }

  /**
   * 批量抽签 - 依次抽取
   */
  async function performBatchDraw() {
    if (isDrawing.value) return;

    try {
      isDrawing.value = true;
      error.value = null;
      selectedStudent.value = null;
      selectedStudents.value = [];

      // 依次抽取每个学生
      for (let i = 0; i < drawCount.value; i++) {
        // 每次抽签前的动画效果（缩短时间）
        const animationDuration = 1000; // 1秒动画
        const interval = 80; // 每80ms切换一次
        const iterations = animationDuration / interval;

        for (let j = 0; j < iterations; j++) {
          if (filteredStudents.value.length > 0) {
            const randomIndex = Math.floor(Math.random() * filteredStudents.value.length);
            const student = filteredStudents.value[randomIndex];
            if (student) {
              selectedStudent.value = student;
            }
            await new Promise(resolve => setTimeout(resolve, interval));
          }
        }

        // 抽取一个学生
        const result = await lotteryApi.drawStudent(
          filterGender.value || undefined,
          filterClass.value || undefined
        );

        // 添加到结果列表
        selectedStudents.value.push(result);
        selectedStudent.value = result;

        // 每次抽取之间的间隔（最后一次不需要等待）
        if (i < drawCount.value - 1) {
          await new Promise(resolve => setTimeout(resolve, 500));
        }
      }

      // 清除单个学生显示，只显示批量结果
      selectedStudent.value = null;

      // 重新加载历史记录
      await loadHistory();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '批量抽签失败';
      console.error('批量抽签失败:', e);
      selectedStudents.value = [];
    } finally {
      isDrawing.value = false;
    }
  }

  /**
   * 重置筛选条件
   */
  function resetFilters() {
    filterGender.value = '';
    filterClass.value = '';
  }

  /**
   * 清除选中的学生
   */
  function clearSelection() {
    selectedStudent.value = null;
    selectedStudents.value = [];
  }

  /**
   * 清空抽签历史
   */
  async function clearHistory() {
    try {
      error.value = null;
      await lotteryApi.clearDrawHistory();
      drawHistory.value = [];
    } catch (e) {
      error.value = e instanceof Error ? e.message : '清空历史记录失败';
      console.error('清空历史记录失败:', e);
    }
  }

  /**
   * 从历史中移除指定学生
   */
  async function removeFromHistory(historyId: number) {
    try {
      error.value = null;
      await lotteryApi.deleteDrawHistory(historyId);
      // 重新加载历史记录
      await loadHistory();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '删除历史记录失败';
      console.error('删除历史记录失败:', e);
    }
  }

  // --- 分组功能 ---
  const groupedStudents = ref<Student[][]>([]);
  const groupSize = ref(2);
  const groupingHistory = ref<import('../types/student').GroupingHistory[]>([]);

  /**
   * 加载分组历史
   */
  async function loadGroupingHistory() {
    try {
      error.value = null;
      const history = await lotteryApi.getGroupingHistory();
      groupingHistory.value = history;
    } catch (e) {
      error.value = e instanceof Error ? e.message : '加载分组历史失败';
      console.error('加载分组历史失败:', e);
    }
  }

  /**
   * 执行随机分组
   */
  async function performGrouping() {
    if (isLoading.value) return;

    try {
      isLoading.value = true;
      error.value = null;
      // 模拟一点延迟，让用户感觉到在计算
      await new Promise(resolve => setTimeout(resolve, 500));

      // 1. 获取当前筛选后的学生列表副本
      const studentsToGroup = [...filteredStudents.value];

      // 2. 洗牌 (Fisher-Yates Shuffle)
      for (let i = studentsToGroup.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        const temp = studentsToGroup[i];
        studentsToGroup[i] = studentsToGroup[j]!;
        studentsToGroup[j] = temp!;
      }

      // 3. 分组
      const groups: Student[][] = [];
      for (let i = 0; i < studentsToGroup.length; i += groupSize.value) {
        groups.push(studentsToGroup.slice(i, i + groupSize.value));
      }

      groupedStudents.value = groups;

      // 4. 保存分组结果到数据库
      if (groups.length > 0) {
        const groupsToSave = groups.map(group =>
          group.map(s => ({
            id: s.id,
            studentId: s.studentId,
            name: s.name,
            gender: s.gender,
            class: s.class,
            major: s.major
          }))
        );
        await lotteryApi.saveGrouping(groupsToSave, groupSize.value);
        // 重新加载分组历史
        await loadGroupingHistory();
      }
    } catch (e) {
      error.value = e instanceof Error ? e.message : '分组失败';
      console.error('分组失败:', e);
    } finally {
      isLoading.value = false;
    }
  }

  /**
   * 清空分组历史
   */
  async function clearGroupingHistory() {
    try {
      error.value = null;
      await lotteryApi.clearGroupingHistory();
      groupingHistory.value = [];
    } catch (e) {
      error.value = e instanceof Error ? e.message : '清空分组历史失败';
      console.error('清空分组历史失败:', e);
    }
  }

  /**
   * 删除指定批次的分组历史
   */
  async function deleteGroupingHistoryBatch(batchId: string) {
    try {
      error.value = null;
      await lotteryApi.deleteGroupingHistoryBatch(batchId);
      // 重新加载分组历史
      await loadGroupingHistory();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '删除分组历史失败';
      console.error('删除分组历史失败:', e);
    }
  }

  // --- 抽奖功能 ---
  const prizeWinners = ref<Student[]>([]);
  const prizeName = ref('');
  const winnerCount = ref(1);
  const prizeHistory = ref<any[]>([]);

  /**
   * 执行抽奖
   */
  async function performPrizeDraw() {
    if (isDrawing.value || !prizeName.value) return;

    try {
      isDrawing.value = true;
      error.value = null;
      prizeWinners.value = [];

      // 依次抽取每个中奖者
      for (let i = 0; i < winnerCount.value; i++) {
        // 动画效果
        const animationDuration = 1000;
        const interval = 80;
        const iterations = animationDuration / interval;

        for (let j = 0; j < iterations; j++) {
          if (filteredStudents.value.length > 0) {
            const randomIndex = Math.floor(Math.random() * filteredStudents.value.length);
            const student = filteredStudents.value[randomIndex];
            if (student) {
              selectedStudent.value = student;
            }
            await new Promise(resolve => setTimeout(resolve, interval));
          }
        }

        // 抽取一个学生
        const result = await lotteryApi.drawStudent(
          filterGender.value || undefined,
          filterClass.value || undefined
        );

        prizeWinners.value.push(result);

        if (i < winnerCount.value - 1) {
          await new Promise(resolve => setTimeout(resolve, 500));
        }
      }

      // 保存抽奖历史
      await lotteryApi.savePrizeDraw(prizeName.value, prizeWinners.value);

      selectedStudent.value = null;

      // 重新加载历史记录
      await loadPrizeHistory();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '抽奖失败';
      console.error('抽奖失败:', e);
      prizeWinners.value = [];
    } finally {
      isDrawing.value = false;
    }
  }

  /**
   * 加载抽奖历史
   */
  async function loadPrizeHistory() {
    try {
      error.value = null;
      const history = await lotteryApi.getPrizeHistory();
      prizeHistory.value = history;
    } catch (e) {
      error.value = e instanceof Error ? e.message : '加载抽奖历史失败';
      console.error('加载抽奖历史失败:', e);
    }
  }

  /**
   * 清空抽奖历史
   */
  async function clearPrizeHistory() {
    try {
      error.value = null;
      await lotteryApi.clearPrizeHistory();
      prizeHistory.value = [];
    } catch (e) {
      error.value = e instanceof Error ? e.message : '清空抽奖历史失败';
      console.error('清空抽奖历史失败:', e);
    }
  }

  /**
   * 删除单条抽奖历史
   */
  async function removePrizeHistory(historyId: number) {
    try {
      error.value = null;
      await lotteryApi.deletePrizeHistory(historyId);
      await loadPrizeHistory();
    } catch (e) {
      error.value = e instanceof Error ? e.message : '删除抽奖历史失败';
      console.error('删除抽奖历史失败:', e);
    }
  }


  return {
    // 状态
    students,
    selectedStudent,
    selectedStudents,
    statistics,
    classList,
    isDrawing,
    isLoading,
    error,

    // 筛选
    filterGender,
    filterClass,
    filteredStudents,
    filteredCount,

    // 历史记录
    drawHistory,
    excludeDrawn,

    // 批量抽签
    drawCount,

    // 方法
    loadStudents,
    loadStatistics,
    loadClassList,
    loadHistory,
    performDraw,
    performBatchDraw,
    resetFilters,
    clearSelection,
    clearHistory,
    removeFromHistory,

    // 分组
    groupedStudents,
    groupSize,
    performGrouping,
    groupingHistory,
    loadGroupingHistory,
    clearGroupingHistory,
    deleteGroupingHistoryBatch,

    // 抽奖
    prizeWinners,
    prizeName,
    winnerCount,
    performPrizeDraw,
    prizeHistory,
    loadPrizeHistory,
    clearPrizeHistory,
    removePrizeHistory

  };
}

