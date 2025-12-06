import type { Student, Statistics } from '../types/student';

// API 基础地址
const API_BASE = 'http://localhost:5000/api/lottery';

/**
 * 获取所有学生
 */
export async function getAllStudents(): Promise<Student[]> {
  const response = await fetch(`${API_BASE}/students`);
  if (!response.ok) {
    throw new Error('获取学生列表失败');
  }
  return response.json();
}

/**
 * 根据ID获取学生
 */
export async function getStudentById(id: number): Promise<Student> {
  const response = await fetch(`${API_BASE}/students/${id}`);
  if (!response.ok) {
    throw new Error('获取学生信息失败');
  }
  return response.json();
}

/**
 * 随机抽取学生
 */
export async function drawStudent(gender?: string, className?: string): Promise<Student> {
  const params = new URLSearchParams();
  if (gender) params.append('gender', gender);
  if (className) params.append('className', className);
  
  const url = `${API_BASE}/draw${params.toString() ? '?' + params.toString() : ''}`;
  const response = await fetch(url);
  
  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || '抽签失败');
  }
  
  return response.json();
}

/**
 * 获取统计信息
 */
export async function getStatistics(): Promise<Statistics> {
  const response = await fetch(`${API_BASE}/stats`);
  if (!response.ok) {
    throw new Error('获取统计信息失败');
  }
  return response.json();
}

/**
 * 获取班级列表
 */
export async function getClassList(): Promise<string[]> {
  const response = await fetch(`${API_BASE}/classes`);
  if (!response.ok) {
    throw new Error('获取班级列表失败');
  }
  return response.json();
}
