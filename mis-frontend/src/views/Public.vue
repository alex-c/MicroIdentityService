<template>
  <main>
    <div id="login-box-container">
      <Box title="μIS - MicroIdentityService">
        <template v-slot:actions><SettingsIcon class="action" @click="showSettingsSidebar" :size="20"/></template>
        <el-form label-position="top" :model="loginForm">
          <el-form-item :label="$t('general.identifier')">
            <el-input v-model="loginForm.identifier"></el-input>
          </el-form-item>
          <el-form-item :label="$t('general.password')" prop="password">
            <el-input type="password" v-model="loginForm.password" autocomplete="off"></el-input>
          </el-form-item>
          <el-form-item>
            <div id="login-form-actions">
              <el-link type="primary">{{ $t('public.passwordForgottenPrompt') }}</el-link>
              <el-button type="primary" @click="signIn">{{ $t('public.signIn') }}</el-button>
            </div>
          </el-form-item>
        </el-form>
      </Box>
    </div>
  </main>
</template>

<script>
import Box from '@/components/shared/Box.vue';
import SettingsIcon from '@/components/shared/icons/SettingsIcon.vue';

import { SET_SETTINGS_DRAWER_OPEN } from '@/store/mutations.js';

export default {
  name: 'Public',
  components: { Box, SettingsIcon },
  data() {
    return {
      loginForm: {
        identifier: '',
        password: '',
      },
    };
  },
  methods: {
    signIn: function() {
      this.$router.push({ path: '/private' });
    },
    showSettingsSidebar: function() {
      this.$store.commit(SET_SETTINGS_DRAWER_OPEN, true);
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

main {
  display: flex;
  justify-content: center;
  align-items: center;
  position: absolute;
  top: 0px;
  left: 0px;
  right: 0px;
  bottom: 0px;
}

#login-box-container {
  width: 480px;
}

#login-form-actions {
  display: flex;
  justify-content: space-between;
}
</style>
