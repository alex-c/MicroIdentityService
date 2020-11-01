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
import Header from '@/views/private/_partials/Header.vue';
import Menu from '@/views/private/_partials/Menu.vue';

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
@import '@/style/colors.scss';

#private {
  height: 100%;
}

main {
  height: calc(100% - 65px);
  display: flex;
}

#content {
  height: 100%;
  flex-grow: 1;
  text-align: left;
}

.standard-page {
  padding: 16px;
}
</style>
