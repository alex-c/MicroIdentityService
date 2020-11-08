<template>
  <div id="app">
    <router-view />
    <SettingsSidebar />
  </div>
</template>

<script>
import SettingsSidebar from '@/views/_partials/SettingsSidebar.vue';

export default {
  name: 'App',
  components: { SettingsSidebar },
};
</script>

<style lang="scss">
@import '@/style/colors.scss';

body {
  margin: 0px;
  background-image: url('./assets/webb.png');
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

#app {
  text-align: center;
  position: absolute;
  top: 0px;
  left: 0px;
  right: 0px;
  bottom: 0px;
}

.left {
  float: left;
}

.right {
  float: right;
}
</style>
