<template>
  <div class="app-container">
    <!-- 头部 -->
    <header class="app-header">
      <h1>🎲 学生抽签系统 v1.0</h1>
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
      <aside class="sidebar left-sidebar">
        <StatsCard :statistics="statistics" />
        <FilterPanel
          v-model:model-gender="filterGender"
          v-model:model-class="filterClass"
          :class-list="classList"
          :filtered-count="filteredCount"
          :disabled="isDrawing"
          @reset="resetFilters"
        />
      </aside>

      <!-- 中间：抽签区域 -->
      <section class="main-content">
        <!-- 模式切换标签 -->
        <div class="mode-tabs">
          <button 
            class="tab-btn" 
            :class="{ active: currentMode === 'lottery' }"
            @click="currentMode = 'lottery'"
          >
            🎲 随机抽签
          </button>
          <button 
            class="tab-btn" 
            :class="{ active: currentMode === 'grouping' }"
            @click="currentMode = 'grouping'"
          >
            👥 全班分组
          </button>
          <button 
            class="tab-btn" 
            :class="{ active: currentMode === 'prize' }"
            @click="currentMode = 'prize'"
          >
            🎁 单项抽奖
          </button>
          <button 
            class="tab-btn" 
            :class="{ active: currentMode === 'multiPrize' }"
            @click="currentMode = 'multiPrize'"
          >
            🏆 多奖项抽奖
          </button>
        </div>

        <div class="content-body">
          <LotteryBox
            v-if="currentMode === 'lottery'"
            :student="selectedStudent"
            :students="selectedStudents"
            :is-drawing="isDrawing"
            :disabled="filteredCount === 0"
            :draw-count="drawCount"
            @draw="performDraw"
            @batch-draw="performBatchDraw"
            @update:draw-count="drawCount = $event"
          />

          <GroupingBox
            v-else-if="currentMode === 'grouping'"
            :groups="groupedStudents"
            v-model:group-size="groupSize"
            :is-grouping="isDrawing" 
            :disabled="filteredCount === 0"
            @group="performGrouping"
          />

          <PrizeDrawBox
            v-else-if="currentMode === 'prize'"
            :winners="prizeWinners"
            :prize-name="prizeName"
            :winner-count="winnerCount"
            :is-drawing="isDrawing"
            :disabled="filteredCount === 0"
            @draw="performPrizeDraw"
            @update:prize-name="prizeName = $event"
            @update:winner-count="winnerCount = $event"
          />

          <MultiPrizeDraw
            v-else-if="currentMode === 'multiPrize'"
            :filter-gender="filterGender"
            :filter-class="filterClass"
          />
        </div>
      </section>

      <!-- 右侧：历史记录 -->
      <aside class="sidebar right-sidebar">
        <!-- 随机抽签历史 -->
        <HistoryPanel
          v-if="currentMode === 'lottery'"
          :history="drawHistory"
          :exclude-drawn="excludeDrawn"
          @clear="clearHistory"
          @remove="removeFromHistory"
          @update:exclude-drawn="excludeDrawn = $event"
        />
        <!-- 分组历史 -->
        <GroupingHistoryPanel
          v-else-if="currentMode === 'grouping'"
          :history="groupingHistory"
          @clear="clearGroupingHistory"
          @remove="deleteGroupingHistoryBatch"
        />
        <!-- 单项抽奖历史 -->
        <PrizeHistoryPanel
          v-else-if="currentMode === 'prize'"
          :history="prizeHistory"
          @clear="clearPrizeHistory"
          @remove="removePrizeHistory"
        />
        <!-- 多奖项抽奖历史 -->
        <PrizeHistoryPanel
          v-else-if="currentMode === 'multiPrize'"
          :history="prizeHistory"
          @clear="clearPrizeHistory"
          @remove="removePrizeHistory"
        />
      </aside>
    </main>

    <!-- 底部 -->
    <footer class="app-footer">
      <p>Powered by Kiro AI Assistant | Vue 3 + TypeScript + C# ASP.NET Core</p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useLottery } from './composables/useLottery';
import StatsCard from './components/StatsCard.vue';
import FilterPanel from './components/FilterPanel.vue';
import GroupingBox from './components/GroupingBox.vue';
import HistoryPanel from './components/HistoryPanel.vue';
import LotteryBox from './components/LotteryBox.vue';
import GroupingHistoryPanel from './components/GroupingHistoryPanel.vue';
import PrizeDrawBox from './components/PrizeDrawBox.vue';
import PrizeHistoryPanel from './components/PrizeHistoryPanel.vue';
import MultiPrizeDraw from './components/MultiPrizeDraw.vue';

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
  loadHistory,
  performDraw,
  performBatchDraw,
  resetFilters,
  clearHistory,
  removeFromHistory,
  
  // 分组
  groupedStudents,
  groupSize,
  performGrouping,
  loadGroupingHistory,
  groupingHistory,
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
} = useLottery();

// 模式切换
const currentMode = ref<'lottery' | 'grouping' | 'prize' | 'multiPrize'>('lottery');

// 组件挂载时加载数据
onMounted(async () => {
  await Promise.all([
    loadStudents(),
    loadStatistics(),
    loadClassList(),
    loadHistory(),
    loadGroupingHistory(),
    loadPrizeHistory()
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
  gap: 1.5rem;
  padding: 2rem;
  max-width: 1600px;
  width: 100%;
  margin: 0 auto;
}

.sidebar {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.left-sidebar {
  flex: 0 0 320px;
}

.right-sidebar {
  flex: 0 0 350px;
}

.main-content {
  flex: 1;
  background: rgba(255, 255, 255, 0.95);
  border-radius: 16px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
  min-width: 0;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.mode-tabs {
  display: flex;
  background: #f8f9fa;
  border-bottom: 1px solid #eee;
}

.tab-btn {
  flex: 1;
  padding: 1rem;
  border: none;
  background: none;
  font-size: 1.1rem;
  font-weight: 500;
  color: #666;
  cursor: pointer;
  transition: all 0.2s;
  border-bottom: 3px solid transparent;
}

.tab-btn:hover {
  color: #667eea;
  background: rgba(102, 126, 234, 0.05);
}

.tab-btn.active {
  color: #667eea;
  border-bottom-color: #667eea;
  background: white;
  font-weight: bold;
}

.content-body {
  flex: 1;
  padding: 1rem;
  overflow-y: auto;
}

.app-footer {
  text-align: center;
  padding: 1.5rem;
  color: white;
  opacity: 0.8;
  font-size: 0.9rem;
}

/* 响应式设计 */
@media (max-width: 1400px) {
  .app-main {
    max-width: 1200px;
  }
  
  .left-sidebar {
    flex: 0 0 280px;
  }
  
  .right-sidebar {
    flex: 0 0 300px;
  }
}

@media (max-width: 1024px) {
  .app-main {
    flex-direction: column;
  }
  
  .left-sidebar,
  .right-sidebar {
    flex: none;
    width: 100%;
  }
}
</style>
