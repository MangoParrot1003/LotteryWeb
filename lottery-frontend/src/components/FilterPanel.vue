<script setup lang="ts">
interface Props {
  className: string
  gender: string
  classList: string[]
  availableCount: number
}

interface Emits {
  (e: 'update:className', value: string): void
  (e: 'update:gender', value: string): void
}

defineProps<Props>()
const emit = defineEmits<Emits>()
</script>

<template>
  <div class="filter-panel">
    <h2 class="panel-title">筛选条件</h2>
    
    <div class="filter-group">
      <label class="filter-label">班级</label>
      <select
        :value="className"
        @change="emit('update:className', ($event.target as HTMLSelectElement).value)"
        class="filter-select"
      >
        <option v-for="cls in classList" :key="cls" :value="cls">
          {{ cls }}
        </option>
      </select>
    </div>

    <div class="filter-group">
      <label class="filter-label">性别</label>
      <div class="gender-options">
        <label class="radio-label">
          <input
            type="radio"
            value="全部"
            :checked="gender === '全部'"
            @change="emit('update:gender', '全部')"
          />
          <span>全部</span>
        </label>
        <label class="radio-label">
          <input
            type="radio"
            value="男"
            :checked="gender === '男'"
            @change="emit('update:gender', '男')"
          />
          <span>男生</span>
        </label>
        <label class="radio-label">
          <input
            type="radio"
            value="女"
            :checked="gender === '女'"
            @change="emit('update:gender', '女')"
          />
          <span>女生</span>
        </label>
      </div>
    </div>

    <div class="count-display">
      <div class="count-number">{{ availableCount }}</div>
      <div class="count-label">符合条件的学生</div>
    </div>
  </div>
</template>

<style scoped>
.filter-panel {
  background: white;
  border-radius: 20px;
  padding: 2rem;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
}

.panel-title {
  font-size: 1.5rem;
  font-weight: 700;
  color: #333;
  margin: 0 0 1.5rem 0;
  padding-bottom: 1rem;
  border-bottom: 2px solid #f0f0f0;
}

.filter-group {
  margin-bottom: 1.5rem;
}

.filter-label {
  display: block;
  font-size: 0.95rem;
  font-weight: 600;
  color: #666;
  margin-bottom: 0.5rem;
}

.filter-select {
  width: 100%;
  padding: 0.75rem 1rem;
  font-size: 1rem;
  border: 2px solid #e0e0e0;
  border-radius: 10px;
  background: white;
  cursor: pointer;
  transition: all 0.3s ease;
}

.filter-select:hover {
  border-color: #667eea;
}

.filter-select:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.gender-options {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.radio-label {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1rem;
  border: 2px solid #e0e0e0;
  border-radius: 10px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.radio-label:hover {
  border-color: #667eea;
  background: rgba(102, 126, 234, 0.05);
}

.radio-label input[type="radio"] {
  width: 18px;
  height: 18px;
  cursor: pointer;
  accent-color: #667eea;
}

.radio-label span {
  font-size: 1rem;
  color: #333;
}

.radio-label:has(input:checked) {
  border-color: #667eea;
  background: rgba(102, 126, 234, 0.1);
}

.count-display {
  margin-top: 2rem;
  padding: 1.5rem;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 16px;
  text-align: center;
  color: white;
}

.count-number {
  font-size: 3rem;
  font-weight: 800;
  line-height: 1;
  margin-bottom: 0.5rem;
}

.count-label {
  font-size: 0.9rem;
  opacity: 0.9;
}

@media (max-width: 1200px) {
  .filter-panel {
    max-width: 600px;
  }
}
</style>
