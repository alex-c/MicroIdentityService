<template>
  <div id="create-role" class="standard-page">
    <Box :title="$t('roles.create')">
      <template slot="actions">
        <BackIcon class="action" :size="20" @click="navigateBack" />
      </template>
      <el-form label-position="top" size="mini" :model="createRoleForm">
        <el-form-item :label="$t('general.name')">
          <el-input v-model="createRoleForm.name" />
        </el-form-item>
        <el-form-item :label="$t('general.domain')">
          <el-select v-model="createRoleForm.domain" clearable>
            <el-option v-for="domain in domains" :key="domain.id" :label="domain.name" :value="domain.id" />
          </el-select>
        </el-form-item>
        <el-form-item class="form-actions">
          <el-button type="primary" icon="el-icon-check" @click="createRole">{{ $t('general.save') }}</el-button>
        </el-form-item>
      </el-form>
    </Box>
  </div>
</template>

<script>
import Api from '@/Api.js';
import GenericErrorHandlingMixin from '@/mixins/GenericErrorHandlingMixin.js';
import Box from '@/components/Box.vue';
import BackIcon from '@/components/icons/BackIcon.vue';

export default {
  name: 'CreateRole',
  mixins: [GenericErrorHandlingMixin],
  components: { Box, BackIcon },
  data() {
    return {
      createRoleForm: {
        name: '',
        domain: null,
      },
      domains: [],
    };
  },
  methods: {
    // API calls
    getDomains: function() {
      Api.domains
        .getDomains('', 1, 10)
        .then(response => {
          this.domains = response.body.data;
        })
        .catch(this.handleHttpError);
    },
    createRole: function() {
      Api.roles
        .createRole(this.createRoleForm.name, this.createRoleForm.domain)
        .then(result => {
          this.$message({
            message: this.$t('roles.createdMessage', this.createRoleForm.name),
            type: 'success',
          });
          this.navigateBack();
        })
        .catch(this.handleHttpError);
    },
    // Navigation
    navigateBack: function() {
      this.$router.push({ path: '/roles' });
    },
  },
  mounted() {
    this.getDomains();
  },
};
</script>

<style lang="scss" scoped>
.el-select {
  width: 100%;
}

.form-actions {
  text-align: right;
}
</style>
