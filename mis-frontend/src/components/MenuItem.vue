<template>
  <div class="menu-item" @click="navigate">
    <el-link :type="type">{{ label }}</el-link>
  </div>
</template>

<script>
import { SET_MENU_DRAWER_OPEN } from '@/store/mutations.js';

export default {
  name: 'MenuItem',
  props: ['target', 'current'],
  data() {
    return {
      label: this.$t(`general.${this.target}`),
      route: `/${this.target}`,
    };
  },
  computed: {
    collapsedUi() {
      return this.$store.state.collapsedUi;
    },
    type() {
      return this.route === this.current ? 'primary' : 'default';
    },
  },
  methods: {
    navigate: function() {
      if (this.route !== this.current) {
        this.$router.push({ path: this.route });
        if (this.collapsedUi) {
          this.$store.commit(SET_MENU_DRAWER_OPEN, false);
        }
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.menu-item {
  padding: 16px 16px 0px;
  & > .el-link {
    font-size: 16px;
  }
}
</style>
