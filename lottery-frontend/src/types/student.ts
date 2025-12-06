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
 * API 响应接口
 */
export interface ApiResponse<T> {
  data?: T;
  message?: string;
  error?: string;
}
