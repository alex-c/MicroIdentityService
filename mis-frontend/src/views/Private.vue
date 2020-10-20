<template>
  <div id="private">
    <header>
      <div id="header-title">
        <i class="el-icon-s-unfold" />
        MicroIdentityServer
      </div>
      <div id="header-options">
        <div class="option"><i class="el-icon-s-tools" @click="showSettingsSidebar" /></div>
      </div>
    </header>
  </div>
</template>

<script>
import { SET_SETTINGS_DRAWER_OPEN } from '@/store/mutations.js';

export default {
  name: 'Private',
  methods: {
    showSettingsSidebar: function() {
      this.$store.commit(SET_SETTINGS_DRAWER_OPEN, true);
    },
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
  font-size: 30px;
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
</style>
