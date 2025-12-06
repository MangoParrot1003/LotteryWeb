<template>
  <div class="app-container">
    <!-- 头部 -->
    <header class="app-header">
      <h1>🎲 学生抽签系统</h1>
      <p class="subtitle">公平 · 随机 · 高效</p>
    </header>

    <!-- 错误提示 -->
    <div v-if="error" class="error-message">
      ⚠️ {{ error }}
      <button @click="error = null" class="close-btn">×</button>
    </div>

    <!-- 主要内容 -->
    <main class="app-main">
      <!-- 左侧：统计和筛选 -->
      <aside class="sidebar">
        <StatsCard :statistics="statistics" />
        <FilterPanel
          v-model:model-gender="filterGender"
          v-model:model-class="filterClass"
          :class-list="classList"
          :filtered-count="filteredCount"
          :disabled="isDrawing"
          @reset="resetFilters"
        />
        <HistoryPanel
          :history="drawHistory"
          :exclude-drawn="excludeDrawn"
          @clear="clearHistory"
          @remove="removeFromHistory"
          @update:exclude-drawn="excludeDrawn = $event"
        />
      </aside>

      <!-- 右侧：抽签区域 -->
      <section class="main-content">
        <LotteryBox
          :student="selectedStudent"
          :students="selectedStudents"
          :is-drawing="isDrawing"
          :disabled="filteredCount === 0"
          :draw-count="drawCount"
          @draw="performDraw"
          @batch-draw="performBatchDraw"
          @update:draw-count="drawCount = $event"
        />
      </section>
    </main>

    <!-- 底部 -->
    <footer class="app-footer">
      <p>Powered by Kiro AI Assistant | Vue 3 + TypeScript + C# ASP.NET Core</p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useLottery } from './composables/useLottery';
import StatsCard from './components/StatsCard.vue';
import FilterPanel from './components/FilterPanel.vue';
import LotteryBox from './components/LotteryBox.vue';
import HistoryPanel from './components/HistoryPanel.vue';

// 使用抽签功能
const {
  selectedStudent,
  selectedStudents,
  statistics,
  classList,
  isDrawing,
  error,
  filterGender,
  filterClass,
  filteredCount,
  drawHistory,
  excludeDrawn,
  drawCount,
  loadStudents,
  loadStatistics,
  loadClassList,
  performDraw,
  performBatchDraw,
  resetFilters,
  clearHistory,
  removeFromHistory
} = useLottery();

// 组件挂载时加载数据
onMounted(async () => {
  await Promise.all([
    loadStudents(),
    loadStatistics(),
    loadClassList()
  ]);
});
</script>

<style scoped>
.app-container {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.app-header {
  text-align: center;
  padding: 2rem;
  color: white;
}

.app-header h1 {
  margin: 0;
  font-size: 2.5rem;
  text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
}

.subtitle {
  margin: 0.5rem 0 0 0;
  font-size: 1.1rem;
  opacity: 0.9;
}

.error-message {
  margin: 1rem auto;
  max-width: 800px;
  padding: 1rem 1.5rem;
  background: #fee;
  border: 2px solid #fcc;
  border-radius: 8px;
  color: #c33;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.close-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  color: #c33;
  cursor: pointer;
  padding: 0;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.app-main {
  flex: 1;
  display: flex;
  gap: 2rem;
  padding: 2rem;
  max-width: 1400px;
  width: 100%;
  margin: 0 auto;
}

.sidebar {
  flex: 0 0 350px;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.main-content {
  flex: 1;
  background: rgba(255, 255, 255, 0.95);
  border-radius: 16px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
}

.app-footer {
  text-align: center;
  padding: 1.5rem;
  color: white;
  opacity: 0.8;
  font-size: 0.9rem;
}

/* 响应式设计 */
@media (max-width: 1024px) {
  .app-main {
    flex-direction: column;
  }
  
  .sidebar {
    flex: none;
    width: 100%;
  }
}
</style>
