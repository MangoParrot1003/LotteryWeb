<template>
  <div class="export-panel">
    <div class="panel-header">
      <h3>📊 数据导出</h3>
      <p class="hint">{{ membershipHint }}</p>
    </div>

    <div class="export-options">
      <button 
        class="export-btn" 
        :disabled="!hasPermission"
        @click="exportData('draw_history')"
      >
        <span class="icon">📋</span>
        <span class="text">导出抽签历史</span>
      </button>

      <button 
        class="export-btn"
        :disabled="!hasPermission"
        @click="exportData('prize_history')"
      >
        <span class="icon">🎁</span>
        <span class="text">导出抽奖记录</span>
      </button>

      <button 
        class="export-btn"
        :disabled="!hasPermission"
        @click="exportData('students')"
      >
        <span class="icon">👨‍🎓</span>
        <span class="text">导出学生名单</span>
      </button>
    </div>

    <div v-if="exporting" class="loading">
      导出中...
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';

const hasPermission = ref(false);
const membership = ref<any>(null);
const exporting = ref(false);

const membershipHint = computed(() => {
  if (!hasPermission.value) {
    return '🔒 需要购买会员才能导出数据';
  }
  
  if (membership.value?.type === 'single') {
    return `✅ 单次导出权限（剩余 ${membership.value.remainingExports} 次）`;
  }
  
  if (membership.value?.type === 'monthly') {
    const expiryDate = new Date(membership.value.expiryTime);
    const days = Math.ceil((expiryDate.getTime() - Date.now()) / (1000 * 60 * 60 * 24));
    return `✅ 包月会员（剩余 ${days} 天）`;
  }
  
  return '✅ 拥有导出权限';
});

// 检查会员状态
const checkMembership = async () => {
  try {
    // 从localStorage获取会员信息
    const membershipData = localStorage.getItem('membership');
    if (membershipData) {
      membership.value = JSON.parse(membershipData);
      hasPermission.value = true;
      return;
    }

    // 也可以从API检查
    const userId = 'default_user';
    const response = await fetch(
      `http://${window.location.hostname}:8502/api/lottery/membership/check/${userId}`
    );
    const data = await response.json();
    
    hasPermission.value = data.hasPermission;
    if (data.membership) {
      membership.value = data.membership;
    }
  } catch (error) {
    console.error('检查会员状态失败:', error);
    hasPermission.value = false;
  }
};

// 导出数据
const exportData = async (exportType: string) => {
  if (!hasPermission.value) {
    alert('需要购买会员才能导出数据！');
    return;
  }

  exporting.value = true;

  try {
    const userId = 'default_user';
    const response = await fetch(`http://${window.location.hostname}:8502/api/lottery/export/excel`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        userId: userId,
        exportType: exportType
      })
    });

    if (response.ok) {
      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${exportType}_${new Date().getTime()}.csv`;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      window.URL.revokeObjectURL(url);

      // 如果是单次导出，减少剩余次数
      if (membership.value?.type === 'single') {
        membership.value.remainingExports--;
        if (membership.value.remainingExports <= 0) {
          hasPermission.value = false;
          localStorage.removeItem('membership');
        } else {
          localStorage.setItem('membership', JSON.stringify(membership.value));
        }
      }
    } else {
      const error = await response.json();
      alert(error.message || '导出失败');
    }
  } catch (error) {
    console.error('导出失败:', error);
    alert('导出失败，请稍后重试');
  } finally {
    exporting.value = false;
  }
};

onMounted(() => {
  checkMembership();
});
</script>

<style scoped>
.export-panel {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.panel-header {
  margin-bottom: 1.5rem;
  text-align: center;
}

.panel-header h3 {
  font-size: 1.3rem;
  color: #333;
  margin: 0 0 0.5rem 0;
}

.hint {
  font-size: 0.9rem;
  color: #666;
  margin: 0;
}

.export-options {
  display: grid;
  gap: 1rem;
}

.export-btn {
  display: flex;
  align-items: center;
  gap: 0.8rem;
  padding: 1rem 1.5rem;
  border: 2px solid #e5e7eb;
  background: white;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s;
  font-size: 1rem;
}

.export-btn:not(:disabled):hover {
  border-color: #667eea;
  background: rgba(102, 126, 234, 0.05);
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.2);
}

.export-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.icon {
  font-size: 1.5rem;
}

.text {
  flex: 1;
  text-align: left;
  font-weight: 500;
  color: #333;
}

.loading {
  text-align: center;
  padding: 1rem;
  color: #667eea;
  font-weight: 500;
}
</style>
