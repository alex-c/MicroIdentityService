<template>
  <div>
    <div id="settings-drawer-overlay" @click="closeDrawer" v-show="drawerOpen" />
    <div id="settings-drawer" :class="{ shown: drawerOpen }">
      <header>
        <span>{{ $t('settings.title') }}</span>
        <i class="el-icon-arrow-right action" @click="closeDrawer" />
      </header>
      <div class="setting">
        <div class="setting-title">{{ $t('settings.language') }}</div>
        <div>
          <el-select v-model="$i18n.locale" @change="selectLanguage" style="width: 100%" size="small">
            <el-option v-for="(lang, i) in languages" :key="`Lang${i}`" :label="lang.label" :value="lang.value">
              <span style="float: left">{{ lang.label }}</span>
              <span style="float: right; color: #8492a6; font-size: 13px">{{ lang.value }}</span>
            </el-option>
          </el-select>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { SET_SETTINGS_DRAWER_OPEN } from '@/store/mutations.js';

import languageIndex from '@/i18n/index.json';

export default {
  name: 'SettingsSidebar',
  data() {
    return {
      languages: languageIndex.languages,
    };
  },
  computed: {
    drawerOpen: function() {
      return this.$store.state.settings.drawerOpen;
    },
  },
  methods: {
    closeDrawer: function() {
      return this.$store.commit(SET_SETTINGS_DRAWER_OPEN, false);
    },
    selectLanguage: function(language) {
      localStorage.setItem('language', language);
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
  position: absolute;
  top: 0px;
  right: 0px;
  bottom: 0px;
  width: 0px;
  background-color: white;
  border-left: 1px solid $mis-color-border;
  box-shadow: -1px 0px 2px 0px $mis-color-shadow;
  transition: width 0.5s;
  overflow: hidden;
  text-align: left;
  &.shown {
    width: 280px;
  }
  & > header {
    display: flex;
    justify-content: space-between;
    padding: 16px;
    background-color: $mis-color-primary;
    color: white;
    border-bottom: 1px solid $mis-color-border;
    & > .action:hover {
      cursor: pointer;
    }
  }
}

.setting {
  padding: 16px;
  & > .setting-title {
    padding: 8px 0px;
  }
}
</style>
