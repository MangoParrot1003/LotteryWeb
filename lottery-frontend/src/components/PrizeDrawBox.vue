<template>
  <div class="prize-draw-box">
    <!-- 抽奖控制区 -->
    <div class="draw-controls">
      <!-- 奖品名称输入 -->
      <div class="prize-name-input">
        <label>奖品名称：</label>
        <input 
          v-model="localPrizeName"
          type="text" 
          placeholder="例如：一等奖" 
          :disabled="isDrawing"
          @input="$emit('update:prizeName', localPrizeName)"
        />
      </div>

      <!-- 中奖人数选择 -->
      <div class="winner-count-selector">
        <label>中奖人数：</label>
        <select 
          :value="winnerCount" 
          @change="$emit('update:winnerCount', Number(($event.target as HTMLSelectElement).value))"
          :disabled="isDrawing"
        >
          <option :value="1">1 人</option>
          <option :value="2">2 人</option>
          <option :value="3">3 人</option>
          <option :value="5">5 人</option>
          <option :value="10">10 人</option>
        </select>
      </div>

      <!-- 抽奖按钮 -->
      <button 
        class="draw-button"
        @click="$emit('draw')"
        :disabled="disabled || isDrawing || !localPrizeName"
        :class="{ drawing: isDrawing }"
      >
        <span v-if="!isDrawing">🎁 开始抽奖</span>
        <span v-else>🎁 抽奖中...</span>
      </button>
    </div>

    <!-- 中奖者展示 -->
    <div v-if="winners.length > 0" class="winners-display">
      <h3 class="winners-title">🎉 {{ prizeName }} - 恭喜中奖！</h3>
      <div class="winners-grid">
        <transition-group name="winner-item">
          <div 
            v-for="(winner, index) in winners" 
            :key="winner.id"
            class="winner-card"
            :class="{ 'is-latest': index === winners.length - 1 && isDrawing }"
          >
            <div class="winner-number">{{ index + 1 }}</div>
            <div class="winner-avatar">{{ winner.name.charAt(0) }}</div>
            <div class="winner-name">{{ winner.name }}</div>
            <div class="winner-id">{{ winner.studentId }}</div>
            <div class="winner-class">{{ winner.class }}</div>
            <div v-if="index === winners.length - 1 && isDrawing" class="latest-badge">
              ✨ 最新
            </div>
          </div>
        </transition-group>
      </div>
    </div>

    <!-- 空状态 -->
    <div v-if="winners.length === 0 && !isDrawing" class="empty-state">
      <div class="empty-icon">🎁</div>
      <p>输入奖品名称和人数，点击按钮开始抽奖</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import type { Student } from '../types/student';

const props = defineProps<{
  winners: Student[];
  prizeName: string;
  winnerCount: number;
  isDrawing: boolean;
  disabled?: boolean;
}>();

const emit = defineEmits<{
  'draw': [];
  'update:prizeName': [value: string];
  'update:winnerCount': [value: number];
}>();

const localPrizeName = ref(props.prizeName);

// 同步 props 变化
watch(() => props.prizeName, (newVal) => {
  localPrizeName.value = newVal;
});
</script>

<style scoped>
.prize-draw-box {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
  padding: 2rem;
}

.draw-controls {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1.2rem;
  width: 100%;
  max-width: 500px;
}

.prize-name-input,
.winner-count-selector {
  display: flex;
  align-items: center;
  gap: 0.8rem;
  font-size: 1rem;
  color: #333;
  width: 100%;
}

.prize-name-input label,
.winner-count-selector label {
  font-weight: 500;
  min-width: 90px;
}

.prize-name-input input {
  flex: 1;
  padding: 0.6rem 1rem;
  font-size: 1rem;
  border: 2px solid #f59e0b;
  border-radius: 8px;
  background: white;
  transition: all 0.2s;
}

.prize-name-input input:focus {
  outline: none;
  border-color: #d97706;
  box-shadow: 0 0 0 3px rgba(245, 158, 11, 0.1);
}

.prize-name-input input:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  background: #f5f5f5;
}

.winner-count-selector select {
  padding: 0.6rem 1rem;
  font-size: 1rem;
  border: 2px solid #f59e0b;
  border-radius: 8px;
  background: white;
  cursor: pointer;
  transition: all 0.2s;
}

.winner-count-selector select:hover:not(:disabled) {
  border-color: #d97706;
}

.winner-count-selector select:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.draw-button {
  padding: 1.2rem 3rem;
  font-size: 1.5rem;
  font-weight: bold;
  color: white;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  border: none;
  border-radius: 50px;
  cursor: pointer;
  transition: all 0.3s;
  box-shadow: 0 4px 15px rgba(245, 158, 11, 0.4);
  margin-top: 0.5rem;
}

.draw-button:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(245, 158, 11, 0.6);
}

.draw-button:active:not(:disabled) {
  transform: translateY(0);
}

.draw-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.draw-button.drawing {
  animation: pulse 1s infinite;
}

@keyframes pulse {
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.05);
  }
}

/* 中奖者展示 */
.winners-display {
  width: 100%;
  max-width: 900px;
}

.winners-title {
  text-align: center;
  color: #d97706;
  font-size: 1.8rem;
  margin: 0 0 1.5rem 0;
  text-shadow: 2px 2px 4px rgba(217, 119, 6, 0.2);
}

.winners-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
  gap: 1rem;
}

.winner-card {
  background: white;
  border-radius: 12px;
  padding: 1.2rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  text-align: center;
  animation: slideIn 0.5s ease-out;
  position: relative;
  transition: all 0.3s;
  border: 2px solid #fef3c7;
}

.winner-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.15);
}

/* 最新中奖者高亮 */
.winner-card.is-latest {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  color: white;
  transform: scale(1.1);
  box-shadow: 0 8px 24px rgba(245, 158, 11, 0.6);
  animation: glow 1.5s ease-in-out infinite, slideIn 0.5s ease-out;
  z-index: 10;
  border-color: #fbbf24;
}

.winner-card.is-latest .winner-number {
  background: white;
  color: #f59e0b;
}

.winner-card.is-latest .winner-avatar {
  background: white;
  color: #f59e0b;
  box-shadow: 0 0 20px rgba(255, 255, 255, 0.5);
}

.winner-card.is-latest .winner-name {
  font-size: 1.3rem;
  font-weight: 700;
}

.winner-card.is-latest .winner-id,
.winner-card.is-latest .winner-class {
  color: rgba(255, 255, 255, 0.9);
}

.latest-badge {
  position: absolute;
  top: -10px;
  right: -10px;
  background: #ffd700;
  color: #333;
  padding: 0.3rem 0.6rem;
  border-radius: 12px;
  font-size: 0.75rem;
  font-weight: bold;
  box-shadow: 0 2px 8px rgba(255, 215, 0, 0.5);
  animation: bounce 0.6s ease-in-out;
}

@keyframes glow {
  0%, 100% {
    box-shadow: 0 8px 24px rgba(245, 158, 11, 0.6);
  }
  50% {
    box-shadow: 0 8px 32px rgba(245, 158, 11, 0.9), 0 0 40px rgba(245, 158, 11, 0.4);
  }
}

@keyframes bounce {
  0%, 100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-5px);
  }
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.winner-number {
  position: absolute;
  top: 8px;
  right: 8px;
  width: 24px;
  height: 24px;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
  font-weight: bold;
}

.winner-avatar {
  width: 60px;
  height: 60px;
  margin: 0 auto 0.8rem;
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  font-weight: bold;
}

.winner-name {
  font-size: 1.1rem;
  font-weight: 600;
  color: #333;
  margin-bottom: 0.4rem;
}

.winner-id {
  font-size: 0.9rem;
  color: #666;
  margin-bottom: 0.2rem;
}

.winner-class {
  font-size: 0.85rem;
  color: #999;
}

/* 空状态 */
.empty-state {
  text-align: center;
  color: #999;
  padding: 3rem;
}

.empty-icon {
  font-size: 4rem;
  margin-bottom: 1rem;
}

.empty-state p {
  font-size: 1.1rem;
}

/* 过渡动画 */
.winner-item-enter-active {
  animation: winnerSlideIn 0.5s ease-out;
}

@keyframes winnerSlideIn {
  from {
    opacity: 0;
    transform: translateY(-20px) scale(0.8);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
  }
}

/* 响应式 */
@media (max-width: 768px) {
  .winners-grid {
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
    gap: 0.8rem;
  }
  
  .winner-card {
    padding: 1rem;
  }
  
  .winner-avatar {
    width: 50px;
    height: 50px;
    font-size: 1.2rem;
  }
}
</style>
