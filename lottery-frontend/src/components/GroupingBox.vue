<template>
  <div class="grouping-box">
    <!-- 控制区 -->
    <div class="grouping-controls">
      <div class="control-item">
        <label>每组人数：</label>
        <input 
          type="number" 
          :value="groupSize" 
          @input="$emit('update:groupSize', Number(($event.target as HTMLInputElement).value))"
          min="2"
          max="50"
          :disabled="isGrouping"
          class="size-input"
        />
      </div>

      <button 
        class="group-button"
        @click="$emit('group')"
        :disabled="disabled || isGrouping"
        :class="{ grouping: isGrouping }"
      >
        <span v-if="!isGrouping">👥 开始分组</span>
        <span v-else>🔄 分组中...</span>
      </button>
    </div>

    <!-- 结果显示 -->
    <div v-if="groups.length > 0" class="groups-container">
      <h3 class="result-title">
        🎉 共分为 {{ groups.length }} 组
        <span class="subtitle">（总人数: {{ totalStudentCount }}）</span>
      </h3>
      
      <div class="groups-grid">
        <transition-group name="list">
          <div 
            v-for="(group, index) in groups" 
            :key="index"
            class="group-card"
            :style="{ '--delay': `${index * 0.05}s` }"
          >
            <div class="group-header">
              <span class="group-number">第 {{ index + 1 }} 组</span>
              <span class="group-count">{{ group.length }}人</span>
            </div>
            
            <div class="group-members">
              <div v-for="student in group" :key="student.id" class="member-item">
                <div class="member-avatar">{{ student.name.charAt(0) }}</div>
                <div class="member-info">
                  <div class="member-name">{{ student.name }}</div>
                  <div class="member-class">{{ student.class }}</div>
                </div>
              </div>
            </div>
          </div>
        </transition-group>
      </div>
    </div>

    <!-- 空状态 -->
    <div v-if="groups.length === 0 && !isGrouping" class="empty-state">
      <div class="empty-icon">🔢</div>
      <p>设置每组人数并开始分组</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { Student } from '../types/student';

const props = defineProps<{
  groups: Student[][];
  groupSize: number;
  isGrouping: boolean;
  disabled?: boolean;
}>();

defineEmits<{
  'group': [];
  'update:groupSize': [value: number];
}>();

const totalStudentCount = computed(() => {
  return props.groups.reduce((total, group) => total + group.length, 0);
});
</script>

<style scoped>
.grouping-box {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
  padding: 1rem;
}

.grouping-controls {
  display: flex;
  align-items: center;
  gap: 1.5rem;
  background: white;
  padding: 1rem 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
}

.control-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-weight: 500;
  color: #333;
}

.size-input {
  width: 80px;
  padding: 0.5rem;
  font-size: 1.1rem;
  border: 2px solid #ddd;
  border-radius: 8px;
  text-align: center;
  transition: all 0.2s;
}

.size-input:focus {
  border-color: #667eea;
  outline: none;
}

.group-button {
  padding: 0.8rem 2rem;
  font-size: 1.1rem;
  font-weight: bold;
  color: white;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border: none;
  border-radius: 50px;
  cursor: pointer;
  transition: all 0.3s;
  box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
}

.group-button:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
}

.group-button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.groups-container {
  width: 100%;
}

.result-title {
  text-align: center;
  color: #333;
  font-size: 1.5rem;
  margin-bottom: 2rem;
}

.subtitle {
  font-size: 1rem;
  color: #666;
  font-weight: normal;
}

.groups-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.5rem;
  padding: 0.5rem;
}

.group-card {
  background: white;
  border-radius: 16px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  transition: all 0.3s;
  animation: cardIn 0.5s ease-out backwards;
  animation-delay: var(--delay);
}

.group-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.12);
}

.group-header {
  background: linear-gradient(to right, #f8f9fa, #fff);
  padding: 1rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #eee;
}

.group-number {
  font-weight: bold;
  color: #667eea;
  font-size: 1.1rem;
}

.group-count {
  font-size: 0.9rem;
  color: #999;
  background: #f0f0f0;
  padding: 0.2rem 0.6rem;
  border-radius: 20px;
}

.group-members {
  padding: 1rem;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  gap: 0.8rem;
  justify-content: flex-start;
}

.member-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.4rem;
  padding: 0.8rem;
  border-radius: 12px;
  background: #fcfcfc;
  transition: all 0.2s;
  min-width: 80px;
  text-align: center;
}

.member-item:hover {
  background: #f0f4ff;
  transform: translateY(-2px);
}

.member-avatar {
  width: 42px;
  height: 42px;
  background: linear-gradient(135deg, #a8c0ff 0%, #3f2b96 100%);
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: bold;
  font-size: 1rem;
}

.member-info {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.2rem;
}

.member-name {
  font-weight: 600;
  color: #333;
  font-size: 0.95rem;
}

.member-class {
  font-size: 0.8rem;
  color: #666;
  background: #eee;
  padding: 0.1rem 0.4rem;
  border-radius: 4px;
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

@keyframes cardIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>
