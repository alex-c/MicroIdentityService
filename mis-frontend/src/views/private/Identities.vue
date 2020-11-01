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
        <!--div class="right">
          <el-button icon="el-icon-download" type="success" size="mini" :disabled="selectedBatch.id === null || selectedBatch.isLocked" @click="checkIn">
            {{ $t('inventory.checkIn') }}
          </el-button>
          <el-button icon="el-icon-upload2" type="success" size="mini" :disabled="selectedBatch.id === null || selectedBatch.isLocked" @click="checkOut">
            {{ $t('inventory.checkOut') }}
          </el-button>
          <router-link :to="{ name: 'editBatch', params: { id: selectedBatch.id } }">
            <el-button icon="el-icon-edit" type="info" size="mini" :disabled="selectedBatch.id === null">{{ $t('general.edit') }}</el-button>
          </router-link>
          <el-button icon="el-icon-view" type="primary" size="mini" :disabled="selectedBatch.id === null" @click="viewLog">{{ $t('inventory.log') }}</el-button>
        </div-->
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
