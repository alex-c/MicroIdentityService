<template>
  <div>
    <header>
      <div id="header-title">MicroIdentityServer</div>
      <div id="header-options">
        <div><el-button size="small">Github</el-button></div>
        <div class="option"><i class="el-icon-s-tools" /></div>
      </div>
    </header>
    <main>
      <div id="login-box">
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
      </div>
    </main>
  </div>
</template>

<script>
export default {
  name: 'Public',
  data() {
    return {
      loginForm: {
        userName: '',
        password: '',
      },
    };
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

header {
  text-align: left;
  background-color: $mis-color-primary;
  overflow: auto;
  color: white;
  border-bottom: 1px solid $mis-color-border;
  box-shadow: 0px 1px 2px 0px $mis-color-shadow;
}

#header-title {
  float: left;
  font-size: 32px;
  padding: 16px;
  height: 32px;
}

#header-options {
  float: right;
  display: flex;
  & > .option {
    font-size: 32px;
    padding: 16px;
    width: 32px;
    height: 32px;
  }
  & > .option:hover {
    background-color: white;
    color: $mis-color-primary;
    cursor: pointer;
  }
}

main {
  padding: 32px 0px;
}

#login-box {
  width: 480px;
  margin: auto;
  text-align: left;
  border: 1px solid $mis-color-border;
  border-radius: 3px;
  box-shadow: 1px 1px 2px 0px $mis-color-shadow;
}

#login-box-header {
  background-color: $mis-color-primary;
  padding: 16px;
  color: white;
  border-bottom: 1px solid $mis-color-border;
}

#login-box-content {
  padding: 16px;
}

#login-form-button {
  text-align: right;
}
</style>
