/**
 * 学生接口
 */
export interface Student {
  id: number;
  serialNumber?: number;
  studentId: string;
  name: string;
  gender?: string;
  major?: string;
  class?: string;
}

/**
 * 统计信息接口
 */
export interface Statistics {
  total: number;
  genderStats: GenderStat[];
  classStats: ClassStat[];
}

/**
 * 性别统计
 */
export interface GenderStat {
  gender: string;
  count: number;
}

/**
 * 班级统计
 */
export interface ClassStat {
  class: string;
  count: number;
}

/**
 * 抽签历史记录接口
 */
export interface DrawHistory {
  id: number;
  studentId: number;
  studentName: string;
  studentNumber: string;
  class?: string;
  gender?: string;
  drawTime: string;
  sessionId?: string;
  isBatch: boolean;
  batchId?: string;
}

/**
 * 分组历史记录接口
 */
export interface GroupingHistory {
  id: number;
  batchId: string;
  groupNumber: number;
  groupSize: number;
  totalGroups: number;
  members: string; // JSON string of members
  groupTime: string;
  sessionId?: string;
}

/**
 * API 响应接口
 */
export interface ApiResponse<T> {
  data?: T;
  message?: string;
  error?: string;
}
