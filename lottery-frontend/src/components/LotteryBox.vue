<template>
  <div class="lottery-box">
    <!-- 抽签按钮 -->
    <button 
      class="draw-button"
      @click="$emit('draw')"
      :disabled="disabled || isDrawing"
      :class="{ drawing: isDrawing }"
    >
      <span v-if="!isDrawing">🎲 开始抽签</span>
      <span v-else>🎲 抽签中...</span>
    </button>

    <!-- 结果显示 -->
    <transition name="fade">
      <div v-if="student" class="result-card" :class="{ drawing: isDrawing }">
        <div class="student-avatar">
          {{ student.name.charAt(0) }}
        </div>
        
        <h2 class="student-name">{{ student.name }}</h2>
        
        <div class="student-info">
          <div class="info-item">
            <span class="info-label">学号</span>
            <span class="info-value">{{ student.studentId }}</span>
          </div>
          
          <div class="info-item">
            <span class="info-label">性别</span>
            <span class="info-value">{{ student.gender || '-' }}</span>
          </div>
          
          <div class="info-item">
            <span class="info-label">专业</span>
            <span class="info-value">{{ student.major || '-' }}</span>
          </div>
          
          <div class="info-item">
            <span class="info-label">班级</span>
            <span class="info-value">{{ student.class || '-' }}</span>
          </div>
        </div>
      </div>
    </transition>

    <!-- 空状态 -->
    <div v-if="!student && !isDrawing" class="empty-state">
      <div class="empty-icon">🎯</div>
      <p>点击上方按钮开始抽签</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Student } from '../types/student';

defineProps<{
  student: Student | null;
  isDrawing: boolean;
  disabled?: boolean;
}>();

defineEmits<{
  'draw': [];
}>();
</script>

<style scoped>
.lottery-box {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
  padding: 2rem;
}

.draw-button {
  padding: 1.2rem 3rem;
  font-size: 1.5rem;
  font-weight: bold;
  color: white;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  border-radius: 50px;
  cursor: pointer;
  transition: all 0.3s;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
}

.draw-button:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
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

.result-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.12);
  min-width: 400px;
  transition: all 0.3s;
}

.result-card.drawing {
  animation: shake 0.1s infinite;
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  25% { transform: translateX(-5px); }
  75% { transform: translateX(5px); }
}

.student-avatar {
  width: 80px;
  height: 80px;
  margin: 0 auto 1rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  font-weight: bold;
}

.student-name {
  text-align: center;
  color: #333;
  font-size: 2rem;
  margin: 0 0 1.5rem 0;
}

.student-info {
  display: flex;
  flex-direction: column;
  gap: 0.8rem;
}

.info-item {
  display: flex;
  justify-content: space-between;
  padding: 0.8rem;
  background: #f8f9fa;
  border-radius: 8px;
}

.info-label {
  color: #666;
  font-weight: 500;
}

.info-value {
  color: #333;
  font-weight: 600;
}

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
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s, transform 0.3s;
}

.fade-enter-from {
  opacity: 0;
  transform: scale(0.9);
}

.fade-leave-to {
  opacity: 0;
  transform: scale(0.9);
}
</style>
