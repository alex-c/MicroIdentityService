<template>
  <div id="identity-list">
    <Box title="Identities">
      <template slot="actions">
        <PlusIcon class="action" :size="20" @click="createIdentity" />
      </template>

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
          <el-table-column prop="roles" :label="$t('general.roles')" :formatter="formatRoles"></el-table-column>
          <el-table-column prop="disabled" :label="$t('identities.status')" :formatter="formatStatus"></el-table-column>
        </el-table>
      </div>

      <!-- Pagination & Actions -->
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
          <el-button icon="el-icon-edit" type="primary" size="mini" :disabled="selectedIdentity.id === null" @click="showIdentityRoles">
            {{ $t('identities.editRoles') }}
          </el-button>
          <el-button icon="el-icon-lock" type="warning" size="mini" :disabled="selectedIdentity.id === null" v-if="selectedIdentity.disabled === false" @click="disableIdentity">
            {{ $t('identities.disable') }}
          </el-button>
          <el-button icon="el-icon-unlock" type="success" size="mini" :disabled="selectedIdentity.id === null" v-else @click="enableIdentity">
            {{ $t('identities.enable') }}
          </el-button>
          <el-button icon="el-icon-delete" type="danger" size="mini" :disabled="selectedIdentity.id === null" @click="deleteIdentity">
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
import PlusIcon from '@/components/icons/PlusIcon.vue';

export default {
  name: 'Identities',
  mixins: [GenericErrorHandlingMixin],
  components: { Box, PlusIcon },
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
    // API calls
    getIdentities: function() {
      this.resetSelectedIdentity();
      Api.identities
        .getIdentities(this.query.page, this.query.elementsPerPage)
        .then(response => {
          this.identities = response.body.data;
          this.totalIdentities = response.body.totalElements;
        })
        .catch(this.handleHttpError);
    },
    enableIdentity: function() {
      this.updateIdentity(false);
    },
    disableIdentity: function() {
      this.updateIdentity(true);
    },
    updateIdentity: function(disabled) {
      Api.identities
        .updateIdentity(this.selectedIdentity.id, disabled)
        .then(this.getIdentities) // TODO: message
        .catch(this.handleHttpError);
    },
    deleteIdentity: function() {
      this.$confirm(this.$t('identities.deleteConfirm', { id: this.selectedIdentity.identifier }), {
        confirmButtonText: this.$t('general.delete'),
        cancelButtonText: this.$t('general.cancel'),
        type: 'warning',
      })
        .then(() => {
          Api.identities
            .deleteIdentity(this.selectedIdentity.id)
            .then(response => {
              this.$message({
                message: this.$t('identities.deletedMessage', this.selectedIdentity.identifier),
                type: 'success',
              });
              this.query.page = 1;
              this.getIdentities();
            })
            .catch(this.handleHttpError);
        })
        .catch(() => {});
    },
    // Table manipulation
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
    // Formatter
    formatRoles: function(identity) {
      return identity.roles.length;
    },
    formatStatus: function(identity) {
      if (identity.disabled) {
        return this.$t('identities.disabled');
      } else {
        return this.$t('identities.enabled');
      }
    },
    // Navigation
    createIdentity: function() {
      this.$router.push({ path: '/identities/create' });
    },
    showIdentityRoles: function() {
      this.$router.push({ name: 'identity' });
    },
  },
  mounted() {
    this.getIdentities();
  },
};
</script>
