<template>
  <div id="menu" :class="{ collapsed: collapsedUi, shown: menuOpen }">
    <MenuItem target="dashboard" :current="current" />
    <MenuItem target="identities" :current="current" />
    <MenuItem target="domains" :current="current" />
    <MenuItem target="roles" :current="current" />
  </div>
</template>

<script>
import MenuItem from '@/components/MenuItem.vue';

export default {
  name: 'Menu',
  components: { MenuItem },
  computed: {
    collapsedUi() {
      return this.$store.state.collapsedUi;
    },
    menuOpen() {
      return this.$store.state.menuOpen;
    },
    current() {
      return this.$route.path;
    },
  },
};
</script>

<style lang="scss" scoped>
@import '@/style/colors.scss';

#menu {
  width: 0px;
  overflow: hidden;
  text-align: left;
  background-color: white;
  border-right: 1px solid $mis-color-border;
  box-shadow: 1px 0px 2px 0px $mis-color-shadow;
  transition: width 0.5s;
  &.collapsed {
    position: absolute;
    top: 65px;
    left: 0px;
    bottom: 0px;
  }
  &.shown {
    width: 200px;
  }
}
</style>
