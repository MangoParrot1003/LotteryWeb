<template>
  <div class="history-panel">
    <div class="panel-header">
      <h3>📜 抽签历史</h3>
      <button 
        v-if="history.length > 0" 
        @click="$emit('clear')" 
        class="clear-btn"
        title="清空历史"
      >
        🗑️ 清空
      </button>
    </div>

    <div class="history-content">
      <div v-if="history.length === 0" class="empty-state">
        <p>暂无抽签记录</p>
      </div>

      <div v-else class="history-list">
        <div 
          v-for="(student, index) in history" 
          :key="student.id"
          class="history-item"
        >
          <div class="item-number">{{ history.length - index }}</div>
          <div class="item-info">
            <div class="item-name">{{ student.name }}</div>
            <div class="item-details">
              {{ student.studentId }} | {{ student.class }}
            </div>
          </div>
          <button 
            @click="$emit('remove', student.id)" 
            class="remove-btn"
            title="移除"
          >
            ×
          </button>
        </div>
      </div>
    </div>

    <div v-if="history.length > 0" class="panel-footer">
      <label class="exclude-checkbox">
        <input 
          type="checkbox" 
          :checked="excludeDrawn"
          @change="$emit('update:excludeDrawn', ($event.target as HTMLInputElement).checked)"
        />
        <span>排除已抽取</span>
      </label>
      <div class="history-count">
        已抽取 {{ history.length }} 人
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Student } from '../types/student';

defineProps<{
  history: Student[];
  excludeDrawn: boolean;
}>();

defineEmits<{
  'clear': [];
  'remove': [id: number];
  'update:excludeDrawn': [value: boolean];
}>();
</script>

<style scoped>
.history-panel {
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  display: flex;
  flex-direction: column;
  max-height: 500px;
}

.panel-header {
  padding: 1rem 1.2rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.panel-header h3 {
  margin: 0;
  font-size: 1.1rem;
}

.clear-btn {
  background: rgba(255, 255, 255, 0.2);
  border: 1px solid rgba(255, 255, 255, 0.3);
  color: white;
  padding: 0.4rem 0.8rem;
  border-radius: 6px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: all 0.2s;
}

.clear-btn:hover {
  background: rgba(255, 255, 255, 0.3);
}

.history-content {
  flex: 1;
  overflow-y: auto;
  padding: 0.5rem;
}

.empty-state {
  text-align: center;
  padding: 2rem;
  color: #999;
}

.history-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.history-item {
  display: flex;
  align-items: center;
  gap: 0.8rem;
  padding: 0.8rem;
  background: #f8f9fa;
  border-radius: 8px;
  transition: all 0.2s;
}

.history-item:hover {
  background: #e9ecef;
  transform: translateX(2px);
}

.item-number {
  width: 28px;
  height: 28px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 0.85rem;
  font-weight: bold;
  flex-shrink: 0;
}

.item-info {
  flex: 1;
  min-width: 0;
}

.item-name {
  font-weight: 600;
  color: #333;
  margin-bottom: 0.2rem;
}

.item-details {
  font-size: 0.85rem;
  color: #666;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
}

.remove-btn {
  width: 24px;
  height: 24px;
  background: #dc3545;
  color: white;
  border: none;
  border-radius: 50%;
  cursor: pointer;
  font-size: 1.2rem;
  line-height: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: all 0.2s;
}

.remove-btn:hover {
  background: #c82333;
  transform: scale(1.1);
}

.panel-footer {
  padding: 1rem 1.2rem;
  background: #f8f9fa;
  border-top: 1px solid #e9ecef;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.exclude-checkbox {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  cursor: pointer;
  user-select: none;
}

.exclude-checkbox input[type="checkbox"] {
  width: 18px;
  height: 18px;
  cursor: pointer;
}

.exclude-checkbox span {
  color: #333;
  font-size: 0.95rem;
}

.history-count {
  color: #666;
  font-size: 0.9rem;
  font-weight: 500;
}

/* 滚动条样式 */
.history-content::-webkit-scrollbar {
  width: 6px;
}

.history-content::-webkit-scrollbar-track {
  background: #f1f1f1;
}

.history-content::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 3px;
}

.history-content::-webkit-scrollbar-thumb:hover {
  background: #555;
}
</style>
