<template>
  <div id="identity-roles" class="standard-page">
    <Box :title="title">
      <template slot="actions">
        <BackIcon class="action" :size="20" @click="navigateBack" />
      </template>
      <div id="filter-bar">
        <el-input placeholder="Filter..." v-model="filter" size="mini" />
      </div>
      <div id="role-editor">
        <div>
          <div class="role-editor-header">{{ $t('identities.availableRoles') }}</div>
          <div class="role-editor-roles">
            <div v-if="availableRoles.length === 0" class="no-roles">
              <EmptyIcon />
            </div>
            <div v-else v-for="role in availableRoles" :key="role.id" class="role">{{ roleName(role) }}</div>
          </div>
        </div>
        <div>
          <div class="role-editor-header">{{ $t('identities.identityRoles') }}</div>
          <div class="role-editor-roles">
            <div v-if="identityRoles.length === 0" class="no-roles">
              <EmptyIcon />
            </div>
            <div v-else v-for="role in identityRoles" :key="role.id" class="role">{{ roleName(role) }}</div>
          </div>
        </div>
      </div>
    </Box>
  </div>
</template>

<script>
import Api from '@/Api.js';
import GenericErrorHandlingMixin from '@/mixins/GenericErrorHandlingMixin.js';
import Box from '@/components/Box.vue';
import BackIcon from '@/components/icons/BackIcon.vue';
import EmptyIcon from '@/components/icons/EmptyIcon.vue';

export default {
  name: 'IdentityRoles',
  components: { Box, BackIcon, EmptyIcon },
  data() {
    return {
      id: this.$route.params.id,
      identity: null,
      domains: [],
      roles: {
        available: [],
        identity: [],
      },
      filter: '',
    };
  },
  computed: {
    title: function() {
      let identifier = this.identity ? ` of ${this.identity.identifier} (${this.identity.id})` : '';
      return this.$t('identities.identityRoles') + identifier;
    },
    availableRoles: function() {
      return this.roles.available.filter(r => r.domainName?.includes(this.filter) || r.name.includes(this.filter));
    },
    identityRoles: function() {
      return this.roles.identity.filter(r => r.domainName?.includes(this.filter) || r.name.includes(this.filter));
    },
  },
  methods: {
    // API calls
    getDomains: function(callback) {
      Api.domains
        .getAll()
        .then(result => {
          this.domains = result.body.data;
          callback();
        })
        .catch(this.handleHttpError);
    },
    getIdentity: function() {
      Api.identities
        .getIdentity(this.id)
        .then(result => {
          this.identity = result.body;
          this.roles.identity = this.enrichRolesWithDomainNames(result.body.roles);
        })
        .catch(this.handleHttpError);
    },
    getRoles: function() {
      Api.roles
        .getAll()
        .then(result => (this.roles.available = this.enrichRolesWithDomainNames(result.body.data)))
        .catch(this.handleHttpError);
    },
    // Navigation
    navigateBack: function() {
      this.$router.push({ path: '/identities' });
    },
    // Other
    enrichRolesWithDomainNames: function(roles) {
      for (let i = 0; i < roles.length; i++) {
        let domain = this.domains.find(d => d.id === roles[i].domainId);
        if (domain !== undefined) {
          roles[i].domainName = domain.name;
        }
      }
      return roles;
    },
    roleName: function(role) {
      let name = role.name;
      if (role.domainName) {
        name = role.domainName + '.' + name;
      }
      return name;
    },
  },
  mounted() {
    this.getDomains(() => {
      this.getRoles();
      this.getIdentity();
    });
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

#role-editor {
  display: flex;
  margin: 0px -8px;
  & > div {
    flex-grow: 1;
    margin: 0px 8px;
  }
}

#filter-bar {
  margin-bottom: 16px;
}

.role-editor-header {
  color: $mis-color-primary;
  font-weight: bold;
  margin-bottom: 8px;
}

.role-editor-roles {
  border: 1px solid $mis-color-shadow;
  border-radius: 2px;
  max-height: 400px;
  overflow: auto;
}

.no-roles {
  color: $mis-color-shadow;
  padding: 8px;
  text-align: center;
}

.role {
  font-size: 14px;
  padding: 8px 8px 0px 8px;
  &:last-child {
    padding-bottom: 8px;
  }
}
</style>
