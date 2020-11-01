<template>
  <div id="identities" class="standard-page">
    <Box title="Identities">
      <!-- Identities Table -->
      <div class="content-row">
        <el-table
          :data="identities"
          stripe
          border
          size="mini"
          :empty-text="$t('general.noData')"
          highlight-current-row
          @current-change="selectIdentity"
          ref="identityTable"
          row-key="id"
        >
          <el-table-column prop="id" :label="$t('general.id')"></el-table-column>
          <el-table-column prop="identifier" :label="$t('general.identifier')"></el-table-column>
          <el-table-column prop="disabled" :label="$t('identities.status')" :formatter="formatStatus"></el-table-column>
        </el-table>
      </div>
      <!-- Pagination & Options -->
      <div class="content-row">
        <div class="left">
          <el-pagination
            background
            layout="prev, pager, next"
            :total="totalIdentities"
            :page-size="query.elementsPerPage"
            :current-page.sync="query.page"
            @current-change="changePage"
          ></el-pagination>
        </div>
        <div class="right">
          <el-button icon="el-icon-lock" type="warning" size="mini" :disabled="selectedIdentity.id === null" v-if="selectedIdentity.disabled === false">
            {{ $t('identities.disable') }}
          </el-button>
          <el-button icon="el-icon-unlock" type="success" size="mini" :disabled="selectedIdentity.id === null" v-else>
            {{ $t('identities.enable') }}
          </el-button>
          <el-button icon="el-icon-delete" type="danger" size="mini" :disabled="selectedIdentity.id === null">
            {{ $t('identities.delete') }}
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

export default {
  name: 'Identities',
  mixins: [GenericErrorHandlingMixin],
  components: { Box },
  data() {
    return {
      query: {
        page: 1,
        elementsPerPage: 10,
        search: '',
      },
      identities: [],
      totalIdentities: 0,
      selectedIdentity: { id: null },
    };
  },
  methods: {
    getIdentities: function() {
      Api.getIdentities(this.query.page, this.query.elementsPerPage)
        .then(response => {
          this.identities = response.body.data;
          this.totalIdentities = response.body.totalElements;
        })
        .catch(this.handleHttpError);
    },
    formatStatus: function(identity) {
      if (identity.disabled) {
        return this.$t('identities.disabled');
      } else {
        return this.$t('identities.enabled');
      }
    },
    selectIdentity: function(identity) {
      this.selectedIdentity = { ...identity };
    },
    resetSelectedIdentity: function() {
      this.$refs['identityTable'].setCurrentRow(1);
      this.selectedIdentity = { id: null };
    },
    changePage: function(page) {
      this.query.page = page;
      this.getIdentities();
    },
  },
  mounted() {
    this.getIdentities();
  },
};
</script>
