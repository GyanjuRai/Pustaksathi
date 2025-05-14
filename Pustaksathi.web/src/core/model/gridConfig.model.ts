export interface MvGridConfig {
  columns?: MvGridColumn[];
  dataSource: {
      data: any[];
      totalRows: number;
  };
  loading: boolean;
  option: MvReqOption;
}

export interface MvReqOption {
  searchText?: string;
  filter?: any;
  offset?: number;
  pageSize?: number;
  sortBy?: string;
  sortOrder?: string;
  tabCategories?: string;
}

export interface MvGridPaging {
  offset?: number;
  pageSize?: number;
  sortBy?: string;
  sortOrder?: string;
}

export interface MvCustomGridRowAction {
  /**
   * Unique identifier for the action.
   * E.g., 'add', 'edit', 'delete', 'export', or any custom action name.
   */
  action: string;

  /**
   * Display label or tooltip text for the action.
   */
  label?: string;

  /**
   * Icon name (Material icons or custom) for the action button.
   */
  icon?: string;

  /**
   * Optional navigation route or URL to trigger when the action is selected.
   */
  route?: string;

  /**
   * Optional callback function to execute custom logic when the action is triggered.
   * The function receives the current row data as its parameter.
   */
  callback?: (row: any) => void;

  /**
   * Flag to indicate if the action should be shown in the grid inline actions.
   */
  showInGrid?: boolean;
}

export interface MvGridRowActionOption {
  /**
   * Optional navigation id, if needed.
   */
  navigationId?: number;

  /**
   * If true, double-click access is bypassed.
   */
  byPassDblClickAccess?: boolean;

  /**
   * Optional default action to trigger on row double-click.
   */
  dblClickNavigationAction?: string;

  /**
   * Default CRUD actions.
   * Set true for the actions that you want to include by default (e.g., add, edit, delete, export).
   * This can be used in simpler grids like for orders and products.
   */
  defaultActions?: {
    add?: boolean;
    edit?: boolean;
    delete?: boolean;
    export?: boolean;
  };

  /**
   * Array of additional custom actions.
   * Use this array to define more actions for modules like inventory, warehouse, supplier, 
   * purchase order, sell order, account payable, account receivable, etc.
   */
  customActions?: MvCustomGridRowAction[];
}


export interface MvGridColumn {
  name: string; // column name
  display?: string; // column display name
  type: string; // Action (For grid inline row actions), Text, Number, Percent, Money, Date, DateTime, CheckBox, Template
  templateColumns?: string[]; // TemplateColumn is the list of columns which is to be shown as template in current column
  /*
      Formats are added by default, use this property if custom format needed
      Defaults: AppConst.data.gridOptions.GridColumnOption.Format
  */
  format?: string;
  cellColor?: string; // change the color of cell text
  cellInfoText?: string; // pass information sentence if needed to show info icon with information in tooltip on hover
  sticky?: boolean; // sticky header - false by default (row Actions should always be sticky)
  disableSort?: boolean; // disable column sort - false by default
  /*
      Cell prefix like $ or Rs 
      Defaults: AppConst.data.gridOptions.GridColumnOption.Prefix
 */
  prefix?: string;
  /*
      Cell suffix like % 
      Defaults: AppConst.data.gridOptions.GridColumnOption.Suffix
 */
  suffix?: string;
  hidden?: boolean; // hidden columns
  width?: number; // column width
  isGroup?: boolean; // is group column
}
