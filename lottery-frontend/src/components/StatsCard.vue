<script setup lang="ts">
interface Props {
  stats: {
    total: number
    available: number
    drawn: number
    genderStats: Record<string, number>
    classStats: Record<string, number>
  }
}

defineProps<Props>()
</script>

<template>
  <div class="stats-card">
    <h2 class="panel-title">数据统计</h2>

    <div class="stat-item">
      <div class="stat-label">总学生数</div>
      <div class="stat-value">{{ stats.total }}</div>
    </div>

    <div class="stat-section">
      <h3 class="section-title">性别分布</h3>
      <div class="stat-grid">
        <div v-for="(count, gender) in stats.genderStats" :key="gender" class="mini-stat">
          <span class="mini-label">{{ gender }}</span>
          <span class="mini-value">{{ count }}</span>
        </div>
      </div>
    </div>

    <div class="stat-section">
      <h3 class="section-title">班级分布</h3>
      <div class="class-list">
        <div v-for="(count, className) in stats.classStats" :key="className" class="class-item">
          <span class="class-name">{{ className }}</span>
          <span class="class-count">{{ count }}人</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.stats-card {
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

.stat-item {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.5rem;
  border-radius: 16px;
  margin-bottom: 1.5rem;
  text-align: center;
}

.stat-label {
  font-size: 0.9rem;
  opacity: 0.9;
  margin-bottom: 0.5rem;
}

.stat-value {
  font-size: 2.5rem;
  font-weight: 800;
}

.stat-section {
  margin-bottom: 1.5rem;
}

.section-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #666;
  margin: 0 0 1rem 0;
}

.stat-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 0.75rem;
}

.mini-stat {
  background: #f5f7fa;
  padding: 1rem;
  border-radius: 10px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.mini-label {
  font-size: 0.95rem;
  color: #666;
}

.mini-value {
  font-size: 1.3rem;
  font-weight: 700;
  color: #667eea;
}

.class-list {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.class-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem 1rem;
  background: #f5f7fa;
  border-radius: 8px;
  font-size: 0.9rem;
}

.class-name {
  color: #333;
  font-weight: 500;
}

.class-count {
  color: #667eea;
  font-weight: 700;
}

@media (max-width: 1200px) {
  .stats-card {
    max-width: 600px;
  }
}
</style>
