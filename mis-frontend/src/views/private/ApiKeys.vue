<template>
  <div id="domains" class="standard-page">
    <Box :title="$t('general.apiKeys')">
      <template slot="actions">
        <PlusIcon class="action" :size="20" @click="createApiKey" />
      </template>

      <!-- Filtering Options -->
      <div class="filter-row">
        <el-input :placeholder="$t('apiKeys.filter')" prefix-icon="el-icon-search" v-model="query.search" size="mini" clearable @change="setSearch" />
      </div>

      <!-- API Keys Table -->
      <div class="content-row">
        <el-table :data="apiKeys" stripe border size="mini" :empty-text="$t('general.noData')" highlight-current-row @current-change="selectApiKey" ref="apiKeyTable" row-key="id">
          <el-table-column prop="id" :label="$t('general.id')" min-width="300"></el-table-column>
          <el-table-column prop="name" :label="$t('general.name')" min-width="150"></el-table-column>
          <el-table-column prop="enabled" :label="$t('general.enabled')" :formatter="formatStatus" min-width="150"></el-table-column>
        </el-table>
      </div>

      <!-- Pagination & Actions -->
      <div class="content-row">
        <div class="left">
          <el-pagination
            background
            layout="prev, pager, next"
            :total="totalApiKeys"
            :page-size="query.elementsPerPage"
            :current-page.sync="query.page"
            @current-change="changePage"
          ></el-pagination>
        </div>
        <div class="right">
          <el-button icon="el-icon-unlock" type="success" size="mini" :disabled="selectedApiKey.id === null" v-if="selectedApiKey.enabled === false" @click="enableApiKey">
            {{ $t('apiKeys.enable') }}
          </el-button>
          <el-button icon="el-icon-lock" type="warning" size="mini" :disabled="selectedApiKey.id === null" v-else @click="disableApiKey">
            {{ $t('apiKeys.disable') }}
          </el-button>
          <el-button icon="el-icon-delete" type="danger" size="mini" :disabled="selectedApiKey.id === null" @click="deleteApiKey">
            {{ $t('apiKeys.delete') }}
          </el-button>
        </div>
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
  name: 'ApiKeys',
  mixins: [GenericErrorHandlingMixin],
  components: { Box, PlusIcon },
  data() {
    return {
      query: {
        page: 1,
        elementsPerPage: 10,
        search: '',
      },
      apiKeys: [],
      totalApiKeys: 0,
      selectedApiKey: { id: null },
    };
  },
  methods: {
    // API calls
    getApiKeys: function() {
      this.resetSelectedApiKey();
      Api.apiKeys
        .getApiKeys(this.query.search, this.query.page, this.query.elementsPerPage)
        .then(response => {
          this.apiKeys = response.body.data;
          this.totalApiKeys = response.body.totalElements;
        })
        .catch(this.handleHttpError);
    },
    createApiKey: function() {
      this.$prompt(this.$t('apiKeys.createPrompt'), this.$t('apiKeys.create'), {
        confirmButtonText: this.$t('general.save'),
        cancelButtonText: this.$t('general.cancel'),
        inputPattern: /^(?!\s*$).+/,
        inputErrorMessage: this.$t('apiKeys.createInputError'),
      })
        .then(({ value }) => {
          Api.apiKeys
            .createApiKey(value)
            .then(result => {
              this.$message({
                message: this.$t('apiKeys.createdMessage', value),
                type: 'success',
              });
              this.getApiKeys();
            })
            .catch(this.handleHttpError);
        })
        .catch(() => {});
    },
    enableApiKey: function() {
      this.updateApiKeyStatus(true);
    },
    disableApiKey: function() {
      this.updateApiKeyStatus(false);
    },
    updateApiKeyStatus: function(enabled) {
      Api.apiKeys
        .updateApiKeyStatus(this.selectedApiKey.id, enabled)
        .then(this.getApiKeys) // TODO: message
        .catch(this.handleHttpError);
    },
    deleteApiKey: function() {
      this.$confirm(this.$t('apiKeys.deleteConfirm', { id: this.selectedApiKey.name }), {
        confirmButtonText: this.$t('general.delete'),
        cancelButtonText: this.$t('general.cancel'),
        type: 'warning',
      })
        .then(() => {
          Api.apiKeys
            .deleteApiKey(this.selectedApiKey.id)
            .then(response => {
              this.$message({
                message: this.$t('apiKeys.deletedMessage', this.selectedApiKey.name),
                type: 'success',
              });
              this.query.page = 1;
              this.getApiKeys();
            })
            .catch(this.handleHttpError);
        })
        .catch(() => {});
    },
    // Table manipulation
    selectApiKey: function(apiKey) {
      this.selectedApiKey = { ...apiKey };
    },
    resetSelectedApiKey: function() {
      this.$refs['apiKeyTable'].setCurrentRow(1);
      this.selectedApiKey = { id: null };
    },
    changePage: function(page) {
      this.query.page = page;
      this.getApiKeys();
    },
    setSearch: function(value) {
      this.query.search = value;
      this.changePage(1);
    },
    // Formatter
    formatStatus: function(apiKey) {
      if (apiKey.enabled) {
        return this.$t('apiKeys.enabled');
      } else {
        return this.$t('apiKeys.disabled');
      }
    },
  },
  mounted() {
    this.getApiKeys();
  },
};
</script>
