<template>
  <div id="roles" class="standard-page">
    <Box :title="$t('general.roles')">
      <template slot="actions">
        <PlusIcon class="action" :size="20" @click="createRole" />
      </template>

      <!-- Filtering Options -->
      <div class="filter-row">
        <el-input :placeholder="$t('roles.filter')" prefix-icon="el-icon-search" v-model="query.search" size="mini" clearable @change="setSearch" />
        <el-select v-model="query.domain" size="mini" clearable @change="setDomain">
          <el-option v-for="(name, id) in domains" :key="id" :label="name" :value="id" />
        </el-select>
        <el-button type="danger" icon="el-icon-close" size="mini" :disabled="query.search == '' && query.domain == ''" @click="clearFilters" />
      </div>

      <!-- Roles Table -->
      <div class="content-row">
        <el-table :data="roles" stripe border size="mini" :empty-text="$t('general.noData')" highlight-current-row @current-change="selectRole" ref="rolesTable" row-key="id">
          <el-table-column prop="id" :label="$t('general.id')"></el-table-column>
          <el-table-column prop="name" :label="$t('general.name')"></el-table-column>
          <el-table-column prop="domainId" :label="$t('general.domain')" :formatter="formatDomain"></el-table-column>
        </el-table>
      </div>

      <!-- Pagination & Options -->
      <div class="content-row">
        <div class="left">
          <el-pagination
            background
            layout="prev, pager, next"
            :total="totalRoles"
            :page-size="query.elementsPerPage"
            :current-page.sync="query.page"
            @current-change="changePage"
          ></el-pagination>
        </div>
        <div class="right"></div>
      </div>
    </Box>
  </div>
</template>

<script>
import Api from '@/Api.js';
import GenericErrorHandlingMixin from '@/mixins/GenericErrorHandlingMixin.js';
import Box from '@/components/Box.vue';
import PlusIcon from '@/components/icons/PlusIcon.vue';

export default {
  name: 'Roles',
  mixins: [GenericErrorHandlingMixin],
  components: { Box, PlusIcon },
  data() {
    return {
      query: {
        page: 1,
        elementsPerPage: 10,
        search: '',
        domain: '',
      },
      domains: {},
      roles: [],
      totalRoles: 0,
      selectedRole: { id: null },
    };
  },
  methods: {
    // API calls
    getDomains: function() {
      Api.domains
        .getDomains('', 1, this.query.elementsPerPage)
        .then(response => {
          const domains = {};
          for (let i = 0; i < response.body.data.length; i++) {
            const domain = response.body.data[i];
            domains[domain.id] = domain.name;
          }
          this.domains = domains;
        })
        .catch(this.handleHttpError);
    },
    getRoles: function() {
      this.resetSelectedRole();
      Api.roles
        .getRoles(this.query.search, this.query.page, this.query.elementsPerPage, this.query.domain)
        .then(response => {
          this.roles = response.body.data;
          this.totalRoles = response.body.totalElements;
        })
        .catch(this.handleHttpError);
    },
    // Table manipulation
    selectRole: function(role) {
      this.selectedRole = { ...role };
    },
    resetSelectedRole: function() {
      this.$refs['rolesTable'].setCurrentRow(1);
      this.selectedRole = { id: null };
    },
    changePage: function(page) {
      this.query.page = page;
      this.getRoles();
    },
    setSearch: function(value) {
      this.query.search = value;
      this.changePage(1);
    },
    setDomain: function(value) {
      this.query.domain = value;
      this.changePage(1);
    },
    clearFilters: function() {
      this.query.search = '';
      this.query.domain = '';
      this.changePage(1);
    },
    // Formatters
    formatDomain: function(role) {
      if (this.domains[role.domainId]) {
        return this.domains[role.domainId];
      } else {
        return null;
      }
    },
    // Navigation
    createRole: function() {
      this.$router.push({ path: '/roles/create' });
    },
  },
  mounted() {
    this.getDomains();
    this.getRoles();
  },
};
</script>

<style lang="scss" scoped>
.filter-row {
  display: flex;
  margin: 0px -4px;
  & > * {
    margin: 0px 4px;
  }
}
</style>
