<template>
  <main>
    <!--div id="login-box">
        <div id="login-box-header">Sign In</div>
        <div id="login-box-content">
          <el-form label-position="top" :model="loginForm">
            <el-form-item label="Name">
              <el-input v-model="loginForm.userName"></el-input>
            </el-form-item>
            <el-form-item label="Password" prop="password">
              <el-input type="password" v-model="loginForm.password" autocomplete="off"></el-input>
            </el-form-item>
            <el-form-item id="login-form-button">
              <el-button type="primary">Sign In</el-button>
            </el-form-item>
          </el-form>
        </div>
      </div-->
    <div id="login-box-container">
      <Box title="MicroIdentityService">
        <template v-slot:actions><i class="el-icon-s-tools action" @click="showSettingsSidebar"/></template>
        <el-form label-position="top" :model="loginForm">
          <el-form-item label="Name">
            <el-input v-model="loginForm.userName"></el-input>
          </el-form-item>
          <el-form-item label="Password" prop="password">
            <el-input type="password" v-model="loginForm.password" autocomplete="off"></el-input>
          </el-form-item>
          <el-form-item id="login-form-button">
            <el-button type="primary" @click="go">Sign In</el-button>
          </el-form-item>
        </el-form>
      </Box>
    </div>
  </main>
</template>

<script>
import Box from '@/components/Box.vue';

import { SET_SETTINGS_DRAWER_OPEN } from '@/store/mutations.js';

export default {
  name: 'Public',
  components: { Box },
  data() {
    return {
      loginForm: {
        userName: '',
        password: '',
      },
    };
  },
  methods: {
    go: function() {
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

#login-form-button {
  text-align: right;
}
</style>
