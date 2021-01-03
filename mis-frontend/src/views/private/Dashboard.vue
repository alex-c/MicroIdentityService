<template>
  <div id="dashboard" class="standard-page">
    <div id="dashboard-cards">
      <DashboardCard entity="identities" />
      <DashboardCard entity="domains" />
      <DashboardCard entity="roles" />
      <DashboardCard entity="apiKeys" path="api-keys" />
    </div>
  </div>
</template>

<script>
import DashboardCard from '@/components/DashboardCard.vue';

export default {
  name: 'Dashboard',
  components: { DashboardCard },
};
</script>

<style lang="scss" scoped>
#dashboard-cards {
  display: flex;
  & > div {
    flex-grow: 1;
  }
}
</style>
