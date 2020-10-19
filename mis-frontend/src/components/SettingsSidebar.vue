<template>
  <div v-show="drawerOpen">
    <div id="settings-drawer-overlay" @click="closeDrawer"></div>
    <div id="settings-drawer">
      <header>
        Settings
        <div class="actions">
          <i class="el-icon-arrow-right action" @click="closeDrawer" />
        </div>
      </header>
    </div>
  </div>
</template>

<script>
import { SET_SETTINGS_DRAWER_OPEN } from '@/store/mutations.js';

export default {
  name: 'SettingsSidebar',
  computed: {
    drawerOpen: function() {
      return this.$store.state.settings.drawerOpen;
    },
  },
  methods: {
    closeDrawer: function() {
      return this.$store.commit(SET_SETTINGS_DRAWER_OPEN, false);
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

#settings-drawer-overlay {
  position: absolute;
  top: 0px;
  left: 0px;
  right: 0px;
  bottom: 0px;
  background-color: $mis-color-shadow;
  opacity: 0.5;
}

#settings-drawer {
  width: 280px;
  position: absolute;
  top: 0px;
  right: 0px;
  bottom: 0px;
  background-color: white;
  border-left: 1px solid $mis-color-border;
  box-shadow: -1px 0px 2px 0px $mis-color-shadow;
  & > header {
    text-align: left;
    background-color: $mis-color-primary;
    padding: 16px;
    border-bottom: 1px solid $mis-color-border;
    & > .actions {
      float: right;
      & > .action:hover {
        cursor: pointer;
      }
    }
  }
}
</style>
