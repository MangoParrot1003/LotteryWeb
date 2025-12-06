<script setup lang="ts">
import type { LotteryHistory } from '../types/student'

interface Props {
  history: LotteryHistory[]
}

interface Emits {
  (e: 'clear'): void
}

defineProps<Props>()
const emit = defineEmits<Emits>()

function formatTime(timestamp: number) {
  const date = new Date(timestamp)
  return date.toLocaleTimeString('zh-CN', { 
    hour: '2-digit', 
    minute: '2-digit',
    second: '2-digit'
  })
}
</script>

<template>
  <div class="history-panel">
    <div class="panel-header">
      <h2 class="panel-title">抽签历史</h2>
      <button
        v-if="history.length > 0"
        @click="emit('clear')"
        class="clear-button"
      >
        清空
      </button>
    </div>

    <div v-if="history.length === 0" class="empty-state">
      <div class="empty-icon">📋</div>
      <p>暂无抽签记录</p>
    </div>

    <div v-else class="history-list">
      <div
        v-for="(item, index) in history"
        :key="item.timestamp"
        class="history-item"
        :style="{ animationDelay: `${index * 0.05}s` }"
      >
        <div class="item-header">
          <span class="item-index">#{{ history.length - index }}</span>
          <span class="item-time">{{ formatTime(item.timestamp) }}</span>
        </div>
        <div class="item-name">{{ item.student.name }}</div>
        <div class="item-details">
          <span>{{ item.student.class }}</span>
          <span class="divider">·</span>
          <span>{{ item.student.gender }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.history-panel {
  background: white;
  border-radius: 20px;
  padding: 2rem;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
  max-height: 600px;
}

.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
  padding-bottom: 1rem;
  border-bottom: 2px solid #f0f0f0;
}

.panel-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #333;
  margin: 0;
}

.clear-button {
  padding: 0.5rem 1rem;
  background: #ff6b6b;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

.clear-button:hover {
  background: #ff5252;
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(255, 107, 107, 0.3);
}

.clear-button:active {
  transform: translateY(0);
}

.empty-state {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  color: #999;
  padding: 3rem 0;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
  opacity: 0.5;
}

.empty-state p {
  margin: 0;
  font-size: 1.1rem;
}

.history-list {
  flex: 1;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.history-list::-webkit-scrollbar {
  width: 6px;
}

.history-list::-webkit-scrollbar-thumb {
  background: #d0d0d0;
  border-radius: 3px;
}

.history-list::-webkit-scrollbar-thumb:hover {
  background: #a0a0a0;
}

.history-item {
  padding: 1rem;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  border-radius: 12px;
  transition: all 0.3s ease;
  animation: slideInRight 0.4s ease-out both;
}

@keyframes slideInRight {
  from {
    opacity: 0;
    transform: translateX(30px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.history-item:hover {
  transform: translateX(-4px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.item-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
  font-size: 0.85rem;
}

.item-index {
  background: #667eea;
  color: white;
  padding: 0.2rem 0.6rem;
  border-radius: 6px;
  font-weight: 600;
}

.item-time {
  color: #666;
}

.item-name {
  font-size: 1.3rem;
  font-weight: 700;
  color: #333;
  margin-bottom: 0.5rem;
}

.item-details {
  font-size: 0.9rem;
  color: #666;
}

.divider {
  margin: 0 0.5rem;
  color: #999;
}

@media (max-width: 1200px) {
  .history-panel {
    max-width: 600px;
    max-height: 400px;
  }
}
</style>
