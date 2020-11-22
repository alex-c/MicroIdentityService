<template>
  <div id="create-identity" class="standard-page">
    <Box :title="$t('identities.create')">
      <template slot="actions">
        <BackIcon class="action" :size="20" @click="navigateBack" />
      </template>
      <el-form label-position="top" size="mini" :model="createIdentityForm">
        <el-form-item :label="$t('general.identifier')">
          <el-input v-model="createIdentityForm.identifier" />
        </el-form-item>
        <el-form-item :label="$t('general.password')">
          <el-input v-model="createIdentityForm.password" type="password" />
        </el-form-item>
        <el-form-item class="form-actions">
          <el-button type="primary" icon="el-icon-check" @click="createIdentity">{{ $t('general.save') }}</el-button>
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
      createIdentityForm: {
        identifier: '',
        password: '',
      },
    };
  },
  methods: {
    // API calls
    createIdentity: function() {
      Api.identities
        .createIdentity(this.createIdentityForm.identifier, this.createIdentityForm.password)
        .then(result => {
          this.$message({
            message: this.$t('identities.createdMessage', this.createIdentityForm.identifier),
            type: 'success',
          });
          this.navigateBack();
        })
        .catch(this.handleHttpError);
    },
    // Navigation
    navigateBack: function() {
      this.$router.push({ path: '/identities' });
    },
  },
};
</script>

<style lang="scss" scoped>
.form-actions {
  text-align: right;
}
</style>
