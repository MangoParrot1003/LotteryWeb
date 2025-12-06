import { ref, computed } from 'vue';
import type { Student, Statistics } from '../types/student';
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
  const drawHistory = ref<Student[]>([]);
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
      const drawnIds = new Set(drawHistory.value.map(s => s.id));
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
      
      // 添加到历史记录
      if (result && !drawHistory.value.find(s => s.id === result.id)) {
        drawHistory.value.push(result);
      }
    } catch (e) {
      error.value = e instanceof Error ? e.message : '抽签失败';
      console.error('抽签失败:', e);
      selectedStudent.value = null;
    } finally {
      isDrawing.value = false;
    }
  }

  /**
   * 批量抽签
   */
  async function performBatchDraw() {
    if (isDrawing.value) return;
    
    try {
      isDrawing.value = true;
      error.value = null;
      selectedStudent.value = null;
      selectedStudents.value = [];

      // 动画效果
      const animationDuration = 2000;
      const interval = 100;
      const iterations = animationDuration / interval;

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

      // 真正的批量抽签
      const results = await lotteryApi.drawMultipleStudents(
        drawCount.value,
        filterGender.value || undefined,
        filterClass.value || undefined
      );
      
      selectedStudents.value = results;
      selectedStudent.value = null;
      
      // 添加到历史记录
      results.forEach(student => {
        if (!drawHistory.value.find(s => s.id === student.id)) {
          drawHistory.value.push(student);
        }
      });
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
  function clearHistory() {
    drawHistory.value = [];
  }

  /**
   * 从历史中移除指定学生
   */
  function removeFromHistory(studentId: number) {
    drawHistory.value = drawHistory.value.filter(s => s.id !== studentId);
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
    performDraw,
    performBatchDraw,
    resetFilters,
    clearSelection,
    clearHistory,
    removeFromHistory
  };
}
