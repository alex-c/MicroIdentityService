<template>
  <div id="domains" class="standard-page">
    <Box :title="$t('general.domains')">
      <template slot="actions">
        <PlusIcon class="action" :size="20" @click="createDomain" />
      </template>
      <!-- Domains Table -->
      <div class="content-row">
        <el-table :data="domains" stripe border size="mini" :empty-text="$t('general.noData')" highlight-current-row @current-change="selectDomain" ref="domainTable" row-key="id">
          <el-table-column prop="id" :label="$t('general.id')"></el-table-column>
          <el-table-column prop="name" :label="$t('general.name')"></el-table-column>
          <el-table-column prop="roles" :label="$t('general.roles')" :formatter="formatRoles"></el-table-column>
        </el-table>
      </div>
      <!-- Pagination & Options -->
      <div class="content-row">
        <div class="left">
          <el-pagination
            background
            layout="prev, pager, next"
            :total="totalDomains"
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
  name: 'Domains',
  mixins: [GenericErrorHandlingMixin],
  components: { Box, PlusIcon },
  data() {
    return {
      query: {
        page: 1,
        elementsPerPage: 10,
        search: '',
      },
      domains: [],
      totalDomains: 0,
      selectedDomain: { id: null },
    };
  },
  methods: {
    getDomains: function() {
      this.resetSelectedDomain();
      Api.domains
        .getDomains(this.query.page, this.query.elementsPerPage)
        .then(response => {
          this.domains = response.body.data;
          this.totalDomains = response.body.totalElements;
        })
        .catch(this.handleHttpError);
    },
    createDomain: function() {
      this.$prompt(this.$t('domains.createPrompt'), this.$t('domains.create'), {
        confirmButtonText: this.$t('general.save'),
        cancelButtonText: this.$t('general.cancel'),
        inputPattern: /^(?!\s*$).+/,
        inputErrorMessage: this.$t('domains.createInputError'),
      })
        .then(({ value }) => {
          Api.domains
            .createDomain(value)
            .then(result => {
              this.$message({
                message: this.$t('domains.createdMessage'),
                type: 'success',
              });
              this.getDomains();
            })
            .catch(this.handleHttpError);
        })
        .catch(() => {});
    },
    formatRoles: function(domain) {
      return 0;
    },
    selectDomain: function(domain) {
      this.selectedDomain = { ...domain };
    },
    resetSelectedDomain: function() {
      this.$refs['domainTable'].setCurrentRow(1);
      this.selectedDomain = { id: null };
    },
    changePage: function(page) {
      this.query.page = page;
      this.getDomains();
    },
  },
  mounted() {
    this.getDomains();
  },
};
</script>
