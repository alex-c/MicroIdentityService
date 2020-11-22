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
            <el-table :data="availableRoles" size="mini" strpie border v-loading="loading">
              <el-table-column prop="selected" width="38">
                <template slot-scope="scope">
                  <el-checkbox v-model="scope.row.selected" />
                </template>
              </el-table-column>
              <el-table-column prop="domainName" :label="$t('general.domain')" />
              <el-table-column prop="name" :label="$t('general.role')" />
            </el-table>
          </div>
        </div>
        <div>
          <div class="role-editor-header">{{ $t('identities.identityRoles') }}</div>
          <div class="role-editor-roles">
            <el-table :data="identityRoles" size="mini" strpie border v-loading="loading">
              <el-table-column prop="domainName" :label="$t('general.domain')" />
              <el-table-column prop="name" :label="$t('general.role')" />
            </el-table>
          </div>
        </div>
      </div>
      <div id="editor-actions">
        <el-button type="warning" size="mini" icon="el-icon-refresh-left" @click="getAll">{{ $t('general.reset') }}</el-button>
        <el-button type="primary" size="mini" icon="el-icon-check" @click="updateRoles">{{ $t('general.save') }}</el-button>
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
      roles: [],
      filter: '',
      loading: false,
    };
  },
  computed: {
    title: function() {
      let identifier = this.identity ? ` of ${this.identity.identifier} (${this.identity.id})` : '';
      return this.$t('identities.identityRoles') + identifier;
    },
    availableRoles: function() {
      return this.roles.filter(r => r.domainName?.includes(this.filter) || r.name.includes(this.filter));
    },
    identityRoles: function() {
      return this.roles.filter(r => r.selected && (r.domainName?.includes(this.filter) || r.name.includes(this.filter)));
    },
  },
  methods: {
    // API calls
    getAll: function() {
      this.loading = true;
      this.roles = [];
      this.filter = '';
      this.getDomains(() => {
        this.getIdentity(() => {
          this.getRoles(() => (this.loading = false));
        });
      });
    },
    getDomains: function(callback) {
      Api.domains
        .getAll()
        .then(result => {
          this.domains = result.body.data;
          callback();
        })
        .catch(this.handleHttpError);
    },
    getIdentity: function(callback) {
      Api.identities
        .getIdentity(this.id)
        .then(result => {
          this.identity = result.body;
          callback();
        })
        .catch(this.handleHttpError);
    },
    getRoles: function(callback) {
      Api.roles
        .getAll()
        .then(result => {
          this.roles = this.enrichRolesWithDomainNameAndSelectionStatus(result.body.data);
          callback();
        })
        .catch(this.handleHttpError);
    },
    updateRoles: function() {
      // TODO: implement
      console.log('TODO');
    },
    // Navigation
    navigateBack: function() {
      this.$router.push({ path: '/identities' });
    },
    // Other
    enrichRolesWithDomainNameAndSelectionStatus: function(roles) {
      for (let i = 0; i < roles.length; i++) {
        const domain = this.domains.find(d => d.id === roles[i].domainId);
        if (domain !== undefined) {
          roles[i].domainName = domain.name;
        }
        const identityRole = this.identity.roles.find(r => r.id === roles[i].id);
        roles[i].selected = identityRole !== undefined;
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
    this.getAll();
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

#editor-actions {
  padding: 16px 0px 0px;
  text-align: right;
}
</style>
