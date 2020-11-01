<template>
  <div id="roles" class="standard-page">
    <Box :title="$t('general.roles')">
      <!-- Roles Table -->
      <div class="content-row">
        <el-table :data="roles" stripe border size="mini" :empty-text="$t('general.noData')" highlight-current-row @current-change="selectRole" ref="rolesTable" row-key="id">
          <el-table-column prop="id" :label="$t('general.id')"></el-table-column>
          <el-table-column prop="name" :label="$t('general.name')"></el-table-column>
          <el-table-column prop="domainId" :label="$t('general.domain')"></el-table-column>
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

export default {
  name: 'Roles',
  mixins: [GenericErrorHandlingMixin],
  components: { Box },
  data() {
    return {
      query: {
        page: 1,
        elementsPerPage: 10,
        search: '',
      },
      roles: [],
      totalRoles: 0,
      selectedRole: { id: null },
    };
  },
  methods: {
    getRoles: function() {
      this.resetSelectedRole();
      Api.roles
        .getRoles(this.query.page, this.query.elementsPerPage)
        .then(response => {
          this.roles = response.body.data;
          this.totalRoles = response.body.totalElements;
        })
        .catch(this.handleHttpError);
    },
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
  },
  mounted() {
    this.getRoles();
  },
};
</script>
