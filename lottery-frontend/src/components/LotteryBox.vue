<template>
  <div class="lottery-box">
    <!-- 抽签控制区 -->
    <div class="draw-controls">
      <!-- 抽签数量选择 -->
      <div class="draw-count-selector">
        <label>抽取数量：</label>
        <select 
          :value="drawCount" 
          @change="$emit('update:drawCount', Number(($event.target as HTMLSelectElement).value))"
          :disabled="isDrawing"
        >
          <option :value="1">1 人</option>
          <option :value="2">2 人</option>
          <option :value="3">3 人</option>
          <option :value="5">5 人</option>
          <option :value="10">10 人</option>
        </select>
      </div>

      <!-- 抽签按钮 -->
      <button 
        class="draw-button"
        @click="drawCount === 1 ? $emit('draw') : $emit('batchDraw')"
        :disabled="disabled || isDrawing"
        :class="{ drawing: isDrawing }"
      >
        <span v-if="!isDrawing">🎲 开始抽签</span>
        <span v-else>🎲 抽签中...</span>
      </button>
    </div>

    <!-- 单个结果显示 -->
    <transition name="fade">
      <div v-if="student" class="result-card" :class="{ drawing: isDrawing }">
        <div class="student-avatar" :title="`姓名: ${student.name}`">
          {{ student.name ? student.name.charAt(0) : '?' }}
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

    <!-- 批量结果显示 -->
    <div v-if="students.length > 0" class="batch-results">
      <h3 class="batch-title">🎉 已抽中 {{ students.length }} 人</h3>
      <div class="batch-grid">
        <transition-group name="batch-item">
          <div 
            v-for="(s, index) in students" 
            :key="s.id"
            class="batch-card"
            :class="{ 'is-latest': index === students.length - 1 && isDrawing }"
          >
            <div class="batch-number">{{ index + 1 }}</div>
            <div class="batch-avatar" :title="`姓名: ${s.name}`">{{ s.name ? s.name.charAt(0) : '?' }}</div>
            <div class="batch-name">{{ s.name }}</div>
            <div class="batch-id">{{ s.studentId }}</div>
            <div class="batch-class">{{ s.class }}</div>
            <div v-if="index === students.length - 1 && isDrawing" class="latest-badge">
              ✨ 最新
            </div>
          </div>
        </transition-group>
      </div>
    </div>

    <!-- 空状态 -->
    <div v-if="!student && students.length === 0 && !isDrawing" class="empty-state">
      <div class="empty-icon">🎯</div>
      <p>点击上方按钮开始抽签</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Student } from '../types/student';

defineProps<{
  student: Student | null;
  students: Student[];
  isDrawing: boolean;
  disabled?: boolean;
  drawCount: number;
}>();

defineEmits<{
  'draw': [];
  'batchDraw': [];
  'update:drawCount': [value: number];
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

.draw-controls {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 1rem;
}

.draw-count-selector {
  display: flex;
  align-items: center;
  gap: 0.8rem;
  font-size: 1rem;
  color: #333;
}

.draw-count-selector label {
  font-weight: 500;
}

.draw-count-selector select {
  padding: 0.5rem 1rem;
  font-size: 1rem;
  border: 2px solid #667eea;
  border-radius: 8px;
  background: white;
  cursor: pointer;
  transition: all 0.2s;
}

.draw-count-selector select:hover:not(:disabled) {
  border-color: #764ba2;
}

.draw-count-selector select:disabled {
  opacity: 0.6;
  cursor: not-allowed;
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

/* 批量结果样式 */
.batch-results {
  width: 100%;
  max-width: 800px;
}

.batch-title {
  text-align: center;
  color: #333;
  font-size: 1.5rem;
  margin: 0 0 1.5rem 0;
}

.batch-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
  gap: 1rem;
}

.batch-card {
  background: white;
  border-radius: 12px;
  padding: 1.2rem;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  text-align: center;
  animation: slideIn 0.5s ease-out;
  position: relative;
  transition: all 0.3s;
}

.batch-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.15);
}

/* 最新抽中的学生高亮效果 */
.batch-card.is-latest {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  transform: scale(1.1);
  box-shadow: 0 8px 24px rgba(102, 126, 234, 0.6);
  animation: glow 1.5s ease-in-out infinite, slideIn 0.5s ease-out;
  z-index: 10;
}

.batch-card.is-latest .batch-number {
  background: white;
  color: #667eea;
}

.batch-card.is-latest .batch-avatar {
  background: white;
  color: #667eea;
  box-shadow: 0 0 20px rgba(255, 255, 255, 0.5);
}

.batch-card.is-latest .batch-name {
  font-size: 1.3rem;
  font-weight: 700;
}

.batch-card.is-latest .batch-id,
.batch-card.is-latest .batch-class {
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
    box-shadow: 0 8px 24px rgba(102, 126, 234, 0.6);
  }
  50% {
    box-shadow: 0 8px 32px rgba(102, 126, 234, 0.9), 0 0 40px rgba(102, 126, 234, 0.4);
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

.batch-number {
  position: absolute;
  top: 8px;
  right: 8px;
  width: 24px;
  height: 24px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
  font-weight: bold;
}

.batch-avatar {
  width: 60px;
  height: 60px;
  margin: 0 auto 0.8rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.5rem;
  font-weight: bold;
}

.batch-name {
  font-size: 1.1rem;
  font-weight: 600;
  color: #333;
  margin-bottom: 0.4rem;
}

.batch-id {
  font-size: 0.9rem;
  color: #666;
  margin-bottom: 0.2rem;
}

.batch-class {
  font-size: 0.85rem;
  color: #999;
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

/* 批量抽签动画 */
.batch-item-enter-active {
  animation: batchSlideIn 0.5s ease-out;
}

@keyframes batchSlideIn {
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
  .batch-grid {
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
    gap: 0.8rem;
  }
  
  .batch-card {
    padding: 1rem;
  }
  
  .batch-avatar {
    width: 50px;
    height: 50px;
    font-size: 1.2rem;
  }
}
</style>
