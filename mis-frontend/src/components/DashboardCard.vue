<template>
  <div class="dashboard-card" @click="navigateToEntitiesView">
    <div class="card-header">{{ entities.length }}</div>
    <div class="card-footer">{{ $t('general.' + entity) }}</div>
  </div>
</template>

<script>
import Api from '@/Api.js';
import GenericErrorHandlingMixin from '@/mixins/GenericErrorHandlingMixin.js';

export default {
  name: 'DashboardCard',
  mixins: [GenericErrorHandlingMixin],
  props: ['entity', 'path'],
  data() {
    return {
      entities: [],
      cleanPath: this.path ? this.path : this.entity,
    };
  },
  methods: {
    getEntities: function() {
      Api[this.entity]
        .getAll()
        .then(response => (this.entities = response.body.data))
        .catch(this.handleHttpError);
    },
    navigateToEntitiesView: function() {
      this.$router.push({ path: '/' + this.cleanPath });
    },
  },
  mounted() {
    this.getEntities();
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

.dashboard-card {
  margin: 20px;
  border: 1px solid $mis-color-border;
  border-radius: 6px;
  text-align: center;
  box-shadow: 1px 1px 2px 0px $mis-color-shadow;
  & > .card-header {
    font-weight: bold;
    padding: 40px 20px;
    border-bottom: 1px solid $mis-color-border;
    background-color: $mis-color-primary;
    color: white;
    text-shadow: 1px 1px 2px $mis-color-shadow;
  }
  & > .card-footer {
    padding: 16px;
    background-color: white;
    border-radius: 0px 0px 6px 6px;
  }
  &:hover {
    cursor: pointer;
    box-shadow: 1px 1px 2px 0px $mis-color-primary;
  }
}
</style>
