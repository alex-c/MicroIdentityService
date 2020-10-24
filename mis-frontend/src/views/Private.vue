<template>
  <div id="private">
    <Header />
    <main>
      <Menu />
      <div id="content"><router-view /></div>
    </main>
  </div>
</template>

<script>
import Header from '@/components/Header.vue';
import Menu from '@/components/Menu.vue';

import { SET_COLLAPSED_UI } from '@/store/mutations.js';

export default {
  name: 'Private',
  components: { Header, Menu },
  methods: {
    fitToScreen: function() {
      this.$store.commit(SET_COLLAPSED_UI, window.innerWidth <= 800);
    },
  },
  created: function() {
    window.addEventListener('resize', this.fitToScreen);
    this.fitToScreen();
  },
  destroyed() {
    window.removeEventListener('resize', this.fitToScreen);
  },
};
</script>

<style lang="scss" scoped>
#content {
  position: absolute;
  top: 65px;
  left: 0px;
  right: 0px;
  bottom: 0px;
}
</style>
