<template>
  <div id="private">
    <Header />
    <main>
      <div id="content" :class="{ collapsed: menuOpen && !collapsedUi }"><router-view /></div>
      <Menu />
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
  computed: {
    collapsedUi() {
      return this.$store.state.ui.collapsedUi;
    },
    menuOpen() {
      return this.$store.state.ui.menuOpen;
    },
  },
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
  position: absolute;
  top: 65px;
  left: 0px;
  right: 0px;
  bottom: 0px;
  text-align: left;
  transition: left 0.5s;
  &.collapsed {
    left: 201px;
  }
}

.standard-page {
  padding: 16px;
}
</style>
