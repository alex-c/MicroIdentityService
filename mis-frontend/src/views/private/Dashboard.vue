<template>
  <div id="dashboard" class="standard-page">
    <div id="dashboard-cards">
      <div>
        <div class="card-header">{{ identities.length }}</div>
        <div class="card-footer">{{ $t('general.identities') }}</div>
      </div>
      <div>
        <div class="card-header">{{ domains.length }}</div>
        <div class="card-footer">{{ $t('general.domains') }}</div>
      </div>
      <div>
        <div class="card-header">{{ roles.length }}</div>
        <div class="card-footer">{{ $t('general.roles') }}</div>
      </div>
    </div>
  </div>
</template>

<script>
import Api from '@/Api.js';
import GenericErrorHandlingMixin from '@/mixins/GenericErrorHandlingMixin.js';

export default {
  name: 'Dashboard',
  mixins: [GenericErrorHandlingMixin],
  data() {
    return {
      identities: [],
      domains: [],
      roles: [],
    };
  },
  methods: {
    getIdentities: function() {
      Api.identities
        .getAllIdentities()
        .then(response => (this.identities = response.body.data))
        .catch(this.handleHttpError);
    },
    getDomains: function() {
      Api.domains
        .getAllDomains()
        .then(response => (this.domains = response.body.data))
        .catch(this.handleHttpError);
    },
    getRoles: function() {
      Api.roles
        .getAllRoles()
        .then(response => (this.roles = response.body.data))
        .catch(this.handleHttpError);
    },
  },
  mounted() {
    this.getIdentities();
    this.getDomains();
    this.getRoles();
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

#dashboard-cards {
  display: flex;
  & > div {
    flex-grow: 1;
    margin: 20px;
    border: 1px solid $mis-color-border;
    border-radius: 6px;
    text-align: center;
    & > .card-header {
      font-weight: bold;
      padding: 40px 20px;
      border-bottom: 1px solid $mis-color-border;
      background-color: $mis-color-primary;
      color: white;
    }
    & > .card-footer {
      padding: 16px;
      background-color: white;
      border-radius: 0px 0px 6px 6px;
    }
  }
}
</style>
