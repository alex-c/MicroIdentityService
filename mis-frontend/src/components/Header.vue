<template>
  <header>
    <div id="header-title">
      <span id="menu-toggle" @click="toggleMenuOpen"><MenuIcon :menu-open="menuOpen" :size="32"/></span>
      <span id="header-name" v-if="!collapsedUi">MicroIdentityServer</span>
      <span id="header-name" v-else>μIS</span>
    </div>
    <div id="header-options">
      <div class="option" @click="signOut"><LogoutIcon :size="32" /></div>
      <div class="option" @click="showSettingsSidebar"><SettingsIcon :size="32" /></div>
    </div>
  </header>
</template>

<script>
import MenuIcon from '@/components/shared/icons/MenuIcon.vue';
import LogoutIcon from '@/components/shared/icons/LogoutIcon.vue';
import SettingsIcon from '@/components/shared/icons/SettingsIcon.vue';

import { SET_MENU_DRAWER_OPEN } from '@/store/mutations.js';
import { SET_SETTINGS_DRAWER_OPEN } from '@/store/mutations.js';

export default {
  name: 'Header',
  components: { MenuIcon, LogoutIcon, SettingsIcon },
  computed: {
    collapsedUi: function() {
      return this.$store.state.collapsedUi;
    },
    menuOpen: function() {
      return this.$store.state.menuOpen;
    },
    menuToggleIcon: function() {
      return {
        'el-icon-s-unfold': !this.menuOpen,
        'el-icon-s-fold': this.menuOpen,
      };
    },
  },
  methods: {
    toggleMenuOpen: function() {
      this.$store.commit(SET_MENU_DRAWER_OPEN, !this.menuOpen);
    },
    showSettingsSidebar: function() {
      this.$store.commit(SET_SETTINGS_DRAWER_OPEN, true);
    },
    signOut: function() {
      this.$router.push({ path: '/' });
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

header {
  text-align: left;
  display: flex;
  justify-content: space-between;
  background-color: $mis-color-primary;
  overflow: auto;
  color: white;
  border-bottom: 1px solid $mis-color-border;
  box-shadow: 0px 1px 2px 0px $mis-color-shadow;
  height: 64px;
}

#header-title {
  font-size: 30px;
  padding: 16px;
  height: 32px;
  display: flex;
  & > #menu-toggle {
    cursor: pointer;
  }
  & > #header-name {
    margin-left: 16px;
  }
}

#header-options {
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
