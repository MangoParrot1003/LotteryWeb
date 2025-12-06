<script setup lang="ts">
import { useLottery } from './composables/useLottery'
import LotteryBox from './components/LotteryBox.vue'
import FilterPanel from './components/FilterPanel.vue'
import HistoryPanel from './components/HistoryPanel.vue'
import StatsCard from './components/StatsCard.vue'

const lottery = useLottery()
</script>

<template>
  <div class="app">
    <div class="background"></div>
    
    <div class="container">
      <header class="header">
        <h1 class="title">🎯 学生抽签系统</h1>
        <p class="subtitle">公平 · 随机 · 透明</p>
      </header>

      <div class="main-content">
        <div class="left-panel">
          <FilterPanel 
            v-model:className="lottery.filterOptions.value.className"
            v-model:gender="lottery.filterOptions.value.gender"
            :classList="lottery.classList.value"
            :availableCount="lottery.availableStudents.value.length"
          />
          
          <StatsCard :stats="lottery.stats.value" />
        </div>

        <div class="center-panel">
          <LotteryBox
            :currentStudent="lottery.currentStudent.value"
            :isAnimating="lottery.isAnimating.value"
            :availableCount="lottery.availableStudents.value.length"
            @draw="lottery.drawStudent"
          />
        </div>

        <div class="right-panel">
          <HistoryPanel
            :history="lottery.history.value"
            @clear="lottery.clearHistory"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.app {
  min-height: 100vh;
  position: relative;
  overflow-x: hidden;
}

.background {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  z-index: -1;
}

.background::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: 
    radial-gradient(circle at 20% 50%, rgba(255, 255, 255, 0.1) 0%, transparent 50%),
    radial-gradient(circle at 80% 80%, rgba(255, 255, 255, 0.1) 0%, transparent 50%);
  animation: pulse 8s ease-in-out infinite;
}

@keyframes pulse {
  0%, 100% { opacity: 1; }
  50% { opacity: 0.5; }
}

.container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 2rem;
}

.header {
  text-align: center;
  margin-bottom: 3rem;
  animation: fadeInDown 0.6s ease-out;
}

@keyframes fadeInDown {
  from {
    opacity: 0;
    transform: translateY(-30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.title {
  font-size: 3.5rem;
  font-weight: 800;
  color: white;
  margin: 0;
  text-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
  letter-spacing: 2px;
}

.subtitle {
  font-size: 1.2rem;
  color: rgba(255, 255, 255, 0.9);
  margin-top: 0.5rem;
  font-weight: 300;
  letter-spacing: 4px;
}

.main-content {
  display: grid;
  grid-template-columns: 300px 1fr 350px;
  gap: 2rem;
  animation: fadeIn 0.8s ease-out 0.2s both;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

.left-panel,
.center-panel,
.right-panel {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

/* 响应式设计 */
@media (max-width: 1200px) {
  .main-content {
    grid-template-columns: 1fr;
    max-width: 600px;
    margin: 0 auto;
  }
  
  .title {
    font-size: 2.5rem;
  }
}

@media (max-width: 768px) {
  .container {
    padding: 1rem;
  }
  
  .title {
    font-size: 2rem;
  }
  
  .subtitle {
    font-size: 1rem;
  }
}
</style>
