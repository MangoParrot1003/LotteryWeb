<script setup lang="ts">
import type { Student } from '../types/student'

interface Props {
  currentStudent: Student | null
  isAnimating: boolean
  availableCount: number
}

interface Emits {
  (e: 'draw'): void
}

defineProps<Props>()
const emit = defineEmits<Emits>()
</script>

<template>
  <div class="lottery-box">
    <div class="lottery-card">
      <div v-if="!currentStudent" class="placeholder">
        <div class="icon">🎲</div>
        <p class="hint">点击下方按钮开始抽签</p>
      </div>

      <div v-else class="student-info" :class="{ animating: isAnimating }">
        <div class="student-name">{{ currentStudent.name }}</div>
        <div class="student-details">
          <div class="detail-item">
            <span class="label">学号</span>
            <span class="value">{{ currentStudent.studentId }}</span>
          </div>
          <div class="detail-item">
            <span class="label">性别</span>
            <span class="value">{{ currentStudent.gender }}</span>
          </div>
          <div class="detail-item">
            <span class="label">专业</span>
            <span class="value">{{ currentStudent.major }}</span>
          </div>
          <div class="detail-item">
            <span class="label">班级</span>
            <span class="value">{{ currentStudent.class }}</span>
          </div>
        </div>
      </div>
    </div>

    <button 
      class="draw-button"
      :disabled="availableCount === 0 || isAnimating"
      @click="emit('draw')"
    >
      <span v-if="isAnimating" class="button-text">
        <span class="spinner"></span>
        抽取中...
      </span>
      <span v-else-if="availableCount === 0" class="button-text">
        无可抽取学生
      </span>
      <span v-else class="button-text">
        🎯 开始抽签
      </span>
    </button>

    <div class="available-count">
      可抽取: <strong>{{ availableCount }}</strong> 人
    </div>
  </div>
</template>

<style scoped>
.lottery-box {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.lottery-card {
  background: white;
  border-radius: 24px;
  padding: 3rem;
  min-height: 400px;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.15);
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.lottery-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 25px 70px rgba(0, 0, 0, 0.2);
}

.placeholder {
  text-align: center;
  color: #999;
}

.icon {
  font-size: 6rem;
  margin-bottom: 1rem;
  animation: float 3s ease-in-out infinite;
}

@keyframes float {
  0%, 100% { transform: translateY(0); }
  50% { transform: translateY(-20px); }
}

.hint {
  font-size: 1.2rem;
  margin: 0;
}

.student-info {
  width: 100%;
  text-align: center;
}

.student-info.animating {
  animation: quickPulse 0.1s ease-in-out;
}

@keyframes quickPulse {
  0%, 100% { opacity: 1; transform: scale(1); }
  50% { opacity: 0.8; transform: scale(0.98); }
}

.student-name {
  font-size: 3.5rem;
  font-weight: 800;
  color: #667eea;
  margin-bottom: 2rem;
  animation: slideInUp 0.5s ease-out;
}

@keyframes slideInUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.student-details {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1.5rem;
  animation: fadeIn 0.6s ease-out 0.2s both;
}

.detail-item {
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  padding: 1rem;
  border-radius: 12px;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.label {
  font-size: 0.85rem;
  color: #666;
  font-weight: 500;
}

.value {
  font-size: 1.1rem;
  color: #333;
  font-weight: 600;
}

.draw-button {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 16px;
  padding: 1.5rem 3rem;
  font-size: 1.5rem;
  font-weight: 700;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 10px 30px rgba(102, 126, 234, 0.4);
  position: relative;
  overflow: hidden;
}

.draw-button::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.2), transparent);
  transition: left 0.5s;
}

.draw-button:hover::before {
  left: 100%;
}

.draw-button:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 15px 40px rgba(102, 126, 234, 0.5);
}

.draw-button:active:not(:disabled) {
  transform: translateY(0);
}

.draw-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
  transform: none;
}

.button-text {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}

.spinner {
  width: 20px;
  height: 20px;
  border: 3px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.available-count {
  text-align: center;
  font-size: 1.2rem;
  color: white;
  font-weight: 500;
}

.available-count strong {
  font-size: 1.5rem;
  color: #ffd700;
  text-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);
}

@media (max-width: 768px) {
  .lottery-card {
    padding: 2rem;
    min-height: 300px;
  }
  
  .student-name {
    font-size: 2.5rem;
  }
  
  .student-details {
    grid-template-columns: 1fr;
  }
  
  .draw-button {
    padding: 1.2rem 2rem;
    font-size: 1.2rem;
  }
}
</style>
