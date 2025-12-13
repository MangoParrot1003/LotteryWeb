<template>
  <div class="paid-prize-container">
    <!-- 功能介绍卡片 -->
    <div class="feature-card">
      <div class="feature-header">
        <h2>💎 尊享付费抽奖</h2>
        <p class="feature-subtitle">解锁高级抽奖功能，享受更多专属特权</p>
      </div>

      <!-- 功能特权列表 -->
      <div class="features-grid">
        <div class="feature-item">
          <div class="feature-icon">🎯</div>
          <h3>定制奖池</h3>
          <p>自定义奖品池，支持更多奖项配置</p>
        </div>
        <div class="feature-item">
          <div class="feature-icon">🎨</div>
          <h3>特效升级</h3>
          <p>炫酷抽奖动画，提升仪式感</p>
        </div>
        <div class="feature-item">
          <div class="feature-icon">📊</div>
          <h3>数据导出</h3>
          <p>导出抽奖记录，支持Excel格式</p>
        </div>
        <div class="feature-item">
          <div class="feature-icon">🏆</div>
          <h3>无限次数</h3>
          <p>不限抽奖次数，随心所欲使用</p>
        </div>
      </div>
    </div>

    <!-- 价格套餐卡片 -->
    <div class="pricing-section">
      <h2 class="pricing-title">选择您的套餐</h2>
      
      <div class="pricing-cards">
        <!-- 单次套餐 -->
        <div class="pricing-card" :class="{ selected: selectedPlan === 'single' }" @click="selectedPlan = 'single'">
          <div class="plan-badge">体验版</div>
          <h3 class="plan-name">单次导出</h3>
          <div class="plan-price">
            <span class="currency">¥</span>
            <span class="amount">1</span>
          </div>
          <ul class="plan-features">
            <li>✓ 1次数据导出权限</li>
            <li>✓ Excel格式导出</li>
            <li>✓ 包含所有抽奖记录</li>
            <li>✗ 有效期限制</li>
          </ul>
          <button class="plan-button" @click.stop="openPaymentModal('single')">
            立即购买
          </button>
        </div>

        <!-- 月度套餐 -->
        <div class="pricing-card popular" :class="{ selected: selectedPlan === 'monthly' }" @click="selectedPlan = 'monthly'">
          <div class="plan-badge popular-badge">最受欢迎</div>
          <h3 class="plan-name">包月会员</h3>
          <div class="plan-price">
            <span class="currency">¥</span>
            <span class="amount">9.9</span>
            <span class="period">/月</span>
          </div>
          <ul class="plan-features">
            <li>✓ 无限次数据导出</li>
            <li>✓ Excel + CSV格式</li>
            <li>✓ 批量导出功能</li>
            <li>✓ 数据统计报表</li>
            <li>✓ 30天有效期</li>
          </ul>
          <button class="plan-button primary" @click.stop="openPaymentModal('monthly')">
            立即购买
          </button>
        </div>
      </div>
    </div>

    <!-- 使用说明 -->
    <div class="instructions-card">
      <h3>📖 使用说明</h3>
      <ol class="instructions-list">
        <li>选择适合您的套餐方案</li>
        <li>点击"立即购买"按钮</li>
        <li>选择支付方式（微信/支付宝）</li>
        <li>扫描二维码完成支付</li>
        <li>支付成功后即可使用付费功能</li>
      </ol>
    </div>

    <!-- 导出功能面板 -->
    <div class="export-section">
      <ExportPanel />
    </div>

    <!-- 支付弹窗 -->
    <PaymentModal 
      v-if="showPaymentModal"
      :plan="currentPlan"
      @close="showPaymentModal = false"
      @payment-success="handlePaymentSuccess"
    />
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import PaymentModal from './PaymentModal.vue';
import ExportPanel from './ExportPanel.vue';

// 选中的套餐
const selectedPlan = ref<'single' | 'monthly'>('monthly');

// 支付弹窗状态
const showPaymentModal = ref(false);
const currentPlan = ref<{
  type: string;
  name: string;
  price: number;
}>({
  type: '',
  name: '',
  price: 0
});

// 打开支付弹窗
const openPaymentModal = (planType: 'single' | 'monthly') => {
  const plans = {
    single: { type: 'single', name: '单次导出', price: 1.0 },
    monthly: { type: 'monthly', name: '包月会员', price: 9.9 }
  };
  
  currentPlan.value = plans[planType];
  showPaymentModal.value = true;
};

// 支付成功处理
const handlePaymentSuccess = () => {
  showPaymentModal.value = false;
  // 这里后续可以添加支付成功后的逻辑
  alert('支付成功！感谢您的支持！');
};
</script>

<style scoped>
.paid-prize-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
}

/* 功能介绍卡片 */
.feature-card {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  border-radius: 20px;
  padding: 3rem;
  color: white;
  margin-bottom: 3rem;
  box-shadow: 0 10px 40px rgba(102, 126, 234, 0.3);
}

.feature-header {
  text-align: center;
  margin-bottom: 3rem;
}

.feature-header h2 {
  font-size: 2.5rem;
  margin: 0 0 1rem 0;
  font-weight: bold;
}

.feature-subtitle {
  font-size: 1.2rem;
  opacity: 0.9;
  margin: 0;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
}

.feature-item {
  text-align: center;
  padding: 1.5rem;
  background: rgba(255, 255, 255, 0.1);
  border-radius: 12px;
  backdrop-filter: blur(10px);
  transition: transform 0.3s, background 0.3s;
}

.feature-item:hover {
  transform: translateY(-5px);
  background: rgba(255, 255, 255, 0.15);
}

.feature-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
}

.feature-item h3 {
  font-size: 1.3rem;
  margin: 0.5rem 0;
}

.feature-item p {
  font-size: 0.95rem;
  opacity: 0.9;
  margin: 0;
}

/* 价格套餐部分 */
.pricing-section {
  margin-bottom: 3rem;
}

.pricing-title {
  text-align: center;
  font-size: 2rem;
  color: #333;
  margin-bottom: 2rem;
}

.pricing-cards {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 2rem;
  margin-bottom: 2rem;
  max-width: 800px;
  margin-left: auto;
  margin-right: auto;
}

.pricing-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  transition: all 0.3s;
  cursor: pointer;
  position: relative;
  border: 3px solid transparent;
}

.pricing-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.15);
}

.pricing-card.selected {
  border-color: #667eea;
  box-shadow: 0 8px 30px rgba(102, 126, 234, 0.3);
}

.pricing-card.popular {
  border-color: #f59e0b;
  transform: scale(1.05);
}

.pricing-card.popular:hover {
  transform: scale(1.08) translateY(-8px);
}

.plan-badge {
  position: absolute;
  top: -12px;
  right: 20px;
  background: #667eea;
  color: white;
  padding: 0.3rem 1rem;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: bold;
}

.popular-badge {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
}

.discount-badge {
  background: linear-gradient(135deg, #10b981 0%, #059669 100%);
}

.plan-name {
  text-align: center;
  font-size: 1.5rem;
  color: #333;
  margin: 1rem 0;
}

.plan-price {
  text-align: center;
  margin: 1.5rem 0;
}

.currency {
  font-size: 1.5rem;
  color: #666;
  vertical-align: top;
}

.amount {
  font-size: 3rem;
  font-weight: bold;
  color: #667eea;
}

.period {
  font-size: 1.2rem;
  color: #666;
}

.save-tag {
  text-align: center;
  color: #10b981;
  font-weight: bold;
  font-size: 1.1rem;
  margin: -0.5rem 0 1rem 0;
}

.plan-features {
  list-style: none;
  padding: 0;
  margin: 2rem 0;
}

.plan-features li {
  padding: 0.6rem 0;
  font-size: 1rem;
  color: #555;
}

.plan-button {
  width: 100%;
  padding: 1rem;
  border: none;
  border-radius: 8px;
  font-size: 1.1rem;
  font-weight: bold;
  cursor: pointer;
  transition: all 0.3s;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
}

.plan-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.plan-button.primary {
  background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
}

.plan-button.primary:hover {
  box-shadow: 0 4px 12px rgba(245, 158, 11, 0.4);
}

/* 使用说明 */
.instructions-card {
  background: white;
  border-radius: 16px;
  padding: 2rem;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.instructions-card h3 {
  color: #333;
  font-size: 1.5rem;
  margin-bottom: 1.5rem;
}

.instructions-list {
  padding-left: 1.5rem;
  margin: 0;
}

.instructions-list li {
  padding: 0.5rem 0;
  font-size: 1.05rem;
  color: #555;
  line-height: 1.6;
}

/* 响应式设计 */
@media (max-width: 768px) {
  .paid-prize-container {
    padding: 1rem;
  }

  .feature-card {
    padding: 2rem 1.5rem;
  }

  .feature-header h2 {
    font-size: 2rem;
  }

  .features-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }

  .pricing-cards {
    grid-template-columns: 1fr;
    gap: 1.5rem;
  }

  .pricing-card.popular {
    transform: scale(1);
  }

  .pricing-card.popular:hover {
    transform: translateY(-8px);
  }
}
</style>
