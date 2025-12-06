<template>
  <div class="stats-card">
    <h3 class="stats-title">📊 数据统计</h3>
    
    <div v-if="statistics" class="stats-content">
      <!-- 总数 -->
      <div class="stat-item total">
        <span class="stat-label">总人数</span>
        <span class="stat-value">{{ statistics.total }}</span>
      </div>

      <!-- 性别统计 -->
      <div class="stat-group">
        <h4>性别分布</h4>
        <div class="stat-list">
          <div 
            v-for="item in statistics.genderStats" 
            :key="item.gender"
            class="stat-item"
          >
            <span class="stat-label">{{ item.gender }}</span>
            <span class="stat-value">{{ item.count }}</span>
          </div>
        </div>
      </div>

      <!-- 班级统计 -->
      <div class="stat-group">
        <h4>班级分布</h4>
        <div class="stat-list">
          <div 
            v-for="item in statistics.classStats" 
            :key="item.class"
            class="stat-item"
          >
            <span class="stat-label">{{ item.class }}</span>
            <span class="stat-value">{{ item.count }}</span>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="stats-loading">
      加载中...
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Statistics } from '../types/student';

defineProps<{
  statistics: Statistics | null;
}>();
</script>

<style scoped>
.stats-card {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.stats-title {
  margin: 0 0 1rem 0;
  color: #333;
  font-size: 1.2rem;
}

.stats-content {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.stat-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.5rem;
  background: #f5f5f5;
  border-radius: 6px;
}

.stat-item.total {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  font-size: 1.1rem;
  font-weight: bold;
}

.stat-label {
  color: inherit;
}

.stat-value {
  font-weight: bold;
  color: inherit;
}

.stat-group {
  margin-top: 0.5rem;
}

.stat-group h4 {
  margin: 0 0 0.5rem 0;
  color: #666;
  font-size: 0.9rem;
}

.stat-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.stats-loading {
  text-align: center;
  color: #999;
  padding: 2rem;
}
</style>
