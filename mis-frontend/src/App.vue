<template>
  <div id="app">
    <router-view />
    <SettingsSidebar />
  </div>
</template>

<script>
import SettingsSidebar from '@/components/SettingsSidebar.vue';

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
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
}

header {
  background-color: $mis-color-primary;
  color: white;
}
</style>
