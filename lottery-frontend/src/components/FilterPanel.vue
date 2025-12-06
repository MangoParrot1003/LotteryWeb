<template>
  <div class="filter-panel">
    <h3 class="filter-title">🎯 筛选条件</h3>
    
    <div class="filter-content">
      <!-- 性别筛选 -->
      <div class="filter-group">
        <label for="gender-filter">性别</label>
        <select 
          id="gender-filter"
          :value="modelGender" 
          @change="updateGender"
          :disabled="disabled"
        >
          <option value="">全部</option>
          <option value="男">男</option>
          <option value="女">女</option>
        </select>
      </div>

      <!-- 班级筛选 -->
      <div class="filter-group">
        <label for="class-filter">班级</label>
        <select 
          id="class-filter"
          :value="modelClass" 
          @change="updateClass"
          :disabled="disabled"
        >
          <option value="">全部</option>
          <option 
            v-for="cls in classList" 
            :key="cls" 
            :value="cls"
          >
            {{ cls }}
          </option>
        </select>
      </div>

      <!-- 重置按钮 -->
      <button 
        class="reset-btn"
        @click="$emit('reset')"
        :disabled="disabled"
      >
        重置
      </button>
    </div>

    <!-- 筛选结果提示 -->
    <div v-if="filteredCount !== null" class="filter-result">
      符合条件的学生：<strong>{{ filteredCount }}</strong> 人
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  modelGender: string;
  modelClass: string;
  classList: string[];
  filteredCount?: number | null;
  disabled?: boolean;
}>();

const emit = defineEmits<{
  'update:modelGender': [value: string];
  'update:modelClass': [value: string];
  'reset': [];
}>();

function updateGender(event: Event) {
  const target = event.target as HTMLSelectElement;
  emit('update:modelGender', target.value);
}

function updateClass(event: Event) {
  const target = event.target as HTMLSelectElement;
  emit('update:modelClass', target.value);
}
</script>

<style scoped>
.filter-panel {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.filter-title {
  margin: 0 0 1rem 0;
  color: #333;
  font-size: 1.2rem;
}

.filter-content {
  display: flex;
  gap: 1rem;
  align-items: flex-end;
  flex-wrap: wrap;
}

.filter-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  flex: 1;
  min-width: 150px;
}

.filter-group label {
  font-weight: 500;
  color: #555;
  font-size: 0.9rem;
}

.filter-group select {
  padding: 0.6rem;
  border: 2px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  cursor: pointer;
  transition: border-color 0.3s;
}

.filter-group select:hover:not(:disabled) {
  border-color: #667eea;
}

.filter-group select:focus {
  outline: none;
  border-color: #667eea;
}

.filter-group select:disabled {
  background: #f5f5f5;
  cursor: not-allowed;
}

.reset-btn {
  padding: 0.6rem 1.5rem;
  background: #f5f5f5;
  border: 2px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s;
}

.reset-btn:hover:not(:disabled) {
  background: #e0e0e0;
  border-color: #999;
}

.reset-btn:disabled {
  cursor: not-allowed;
  opacity: 0.5;
}

.filter-result {
  margin-top: 1rem;
  padding: 0.8rem;
  background: #f0f7ff;
  border-left: 4px solid #667eea;
  border-radius: 4px;
  color: #333;
}

.filter-result strong {
  color: #667eea;
  font-size: 1.1rem;
}
</style>
