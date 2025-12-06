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
  const statistics = ref<Statistics | null>(null);
  const classList = ref<string[]>([]);
  const isDrawing = ref(false);
  const isLoading = ref(false);
  const error = ref<string | null>(null);

  // 筛选条件
  const filterGender = ref<string>('');
  const filterClass = ref<string>('');

  // 计算属性
  const filteredStudents = computed(() => {
    let result = students.value;
    
    if (filterGender.value) {
      result = result.filter(s => s.gender === filterGender.value);
    }
    
    if (filterClass.value) {
      result = result.filter(s => s.class === filterClass.value);
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
    } catch (e) {
      error.value = e instanceof Error ? e.message : '抽签失败';
      console.error('抽签失败:', e);
      selectedStudent.value = null;
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
  }

  return {
    // 状态
    students,
    selectedStudent,
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
    
    // 方法
    loadStudents,
    loadStatistics,
    loadClassList,
    performDraw,
    resetFilters,
    clearSelection
  };
}
