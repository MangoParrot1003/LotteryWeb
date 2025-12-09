import type { Student, Statistics, DrawHistory } from '../types/student';

// API 基础地址 - 自动适配本机和局域网访问
// 如果通过 IP 访问前端，后端也使用相同的 IP
// 如果通过 localhost 访问前端，后端也使用 localhost
const getApiBase = () => {
  const hostname = window.location.hostname;
  return `http://${hostname}:8502/api/lottery`;
};

const API_BASE = getApiBase();

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

/**
 * 批量随机抽取学生
 */
export async function drawMultipleStudents(
  count: number,
  gender?: string,
  className?: string
): Promise<Student[]> {
  const params = new URLSearchParams();
  params.append('count', count.toString());
  if (gender) params.append('gender', gender);
  if (className) params.append('className', className);

  const url = `${API_BASE}/draw-multiple?${params.toString()}`;
  const response = await fetch(url);

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || '批量抽签失败');
  }

  return response.json();
}

/**
 * 获取抽签历史记录
 */
export async function getDrawHistory(sessionId?: string, limit: number = 100): Promise<DrawHistory[]> {
  const params = new URLSearchParams();
  if (sessionId) params.append('sessionId', sessionId);
  params.append('limit', limit.toString());

  const url = `${API_BASE}/history?${params.toString()}`;
  const response = await fetch(url);

  if (!response.ok) {
    throw new Error('获取历史记录失败');
  }

  return response.json();
}

/**
 * 清空抽签历史记录
 */
export async function clearDrawHistory(sessionId?: string): Promise<void> {
  const params = new URLSearchParams();
  if (sessionId) params.append('sessionId', sessionId);

  const url = `${API_BASE}/history?${params.toString()}`;
  const response = await fetch(url, { method: 'DELETE' });

  if (!response.ok) {
    throw new Error('清空历史记录失败');
  }
}

/**
 * 删除单条历史记录
 */
export async function deleteDrawHistory(historyId: number): Promise<void> {
  const url = `${API_BASE}/history/${historyId}`;
  const response = await fetch(url, { method: 'DELETE' });

  if (!response.ok) {
    throw new Error('删除历史记录失败');
  }
}

// ========== 分组历史 API ==========

/**
 * 保存分组结果
 */
export async function saveGrouping(
  groups: Array<Array<{ id: number; studentId: string; name: string; gender?: string; class?: string; major?: string }>>,
  groupSize: number,
  sessionId?: string
): Promise<void> {
  const response = await fetch(`${API_BASE}/grouping`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      groups,
      groupSize,
      sessionId
    }),
  });

  if (!response.ok) {
    throw new Error('保存分组失败');
  }
}

/**
 * 获取分组历史记录
 */
export async function getGroupingHistory(sessionId?: string, limit: number = 10): Promise<import('../types/student').GroupingHistory[]> {
  const params = new URLSearchParams();
  if (sessionId) params.append('sessionId', sessionId);
  params.append('limit', limit.toString());

  const url = `${API_BASE}/grouping-history?${params.toString()}`;
  const response = await fetch(url);

  if (!response.ok) {
    throw new Error('获取分组历史失败');
  }

  return response.json();
}

/**
 * 清空分组历史记录
 */
export async function clearGroupingHistory(sessionId?: string): Promise<void> {
  const params = new URLSearchParams();
  if (sessionId) params.append('sessionId', sessionId);

  const url = `${API_BASE}/grouping-history?${params.toString()}`;
  const response = await fetch(url, { method: 'DELETE' });

  if (!response.ok) {
    throw new Error('清空分组历史失败');
  }
}

/**
 * 删除指定批次的分组历史
 */
export async function deleteGroupingHistoryBatch(batchId: string): Promise<void> {
  const url = `${API_BASE}/grouping-history/${batchId}`;
  const response = await fetch(url, { method: 'DELETE' });

  if (!response.ok) {
    throw new Error('删除分组历史失败');
  }
}

// ========== 抽奖历史 API ==========

/**
 * 保存抽奖结果
 */
export async function savePrizeDraw(
  prizeName: string,
  winners: Student[],
  sessionId?: string
): Promise<void> {
  const response = await fetch(`${API_BASE}/prize-draw`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      prizeName,
      winners,
      sessionId
    }),
  });

  if (!response.ok) {
    throw new Error('保存抽奖结果失败');
  }
}

/**
 * 获取抽奖历史记录
 */
export async function getPrizeHistory(sessionId?: string, limit: number = 10): Promise<any[]> {
  const params = new URLSearchParams();
  if (sessionId) params.append('sessionId', sessionId);
  params.append('limit', limit.toString());

  const url = `${API_BASE}/prize-history?${params.toString()}`;
  const response = await fetch(url);

  if (!response.ok) {
    throw new Error('获取抽奖历史失败');
  }

  return response.json();
}

/**
 * 清空抽奖历史记录
 */
export async function clearPrizeHistory(sessionId?: string): Promise<void> {
  const params = new URLSearchParams();
  if (sessionId) params.append('sessionId', sessionId);

  const url = `${API_BASE}/prize-history?${params.toString()}`;
  const response = await fetch(url, { method: 'DELETE' });

  if (!response.ok) {
    throw new Error('清空抽奖历史失败');
  }
}

/**
 * 删除单条抽奖历史记录
 */
export async function deletePrizeHistory(historyId: number): Promise<void> {
  const url = `${API_BASE}/prize-history/${historyId}`;
  const response = await fetch(url, { method: 'DELETE' });

  if (!response.ok) {
    throw new Error('删除抽奖历史失败');
  }
}
