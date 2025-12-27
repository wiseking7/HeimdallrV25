using System.ComponentModel;

namespace Heimdallr.UI.Enums;

/// <summary>
/// 다양한 종류의 아이콘을 나타내는 열거형(Enum) 입니다.
/// 각 아이콘은 Geometry 타입으로 표현되는 벡터 그래픽 형태로, UI 요소에 시각적 아이콘으로 사용됩니다.
/// </summary>
public enum IconType
{
  [Description("계정")]
  Account,

  [Description("계정")]
  AccountA,

  [Description("박스형태계정")]
  AccountBox,

  [Description("계정 원형")]
  AccountCircle,

  [Description("계정 그룹")]
  AccountGroup,

  [Description("여러 계정")]
  AccountMultipleOutline,

  [Description("활성")]
  Acitve,

  [Description("활성")]
  AcitveA,

  [Description("활성")]
  AcitveB,

  [Description("주소")]
  Address,

  [Description("조정")]
  AdJustment,

  [Description("분석")]
  Analytic,

  [Description("Apple")]
  Apple,

  [Description("모든 화살표")]
  ArrowAll,

  [Description("승인")]
  Approval,

  [Description("화살표 아래 박스")]
  ArrowDownBox,

  [Description("화살표 아래")]
  ArrowDown,

  [Description("화살표 아래")]
  ArrowDownBold,

  [Description("화살표 왼쪽")]
  ArrowLeft,

  [Description("화살표 왼쪽 A")]
  ArrowLefA,

  [Description("굵은 화살표 왼쪽")]
  ArrowLeftBold,

  [Description("좌측,우측 연결 화살표 왼쪽")]
  ArrowLeftRight,

  [Description("얇은 화살표 왼쪽")]
  ArrowLeftThin,

  [Description("화살표 오른쪽")]
  ArrowRight,

  [Description("화살표 오른쪽 A")]
  ArrowRightA,

  [Description("굵은 화살표 오른쪽")]
  ArrowRightBold,

  [Description("얇은 화살표 오른쪽")]
  ArrowRightThin,

  [Description("굵은 화살표 위")]
  ArrowUpBold,

  [Description("화살표 위")]
  ArrowUp,

  [Description("왼쪽방향")]
  Arrow_Triangle_Left,

  [Description("삼각형 화살표 오른쪽")]
  Arrow_Triangle_Right,

  [Description("삼각형 화살표 위")]
  Arrow_Triangle_Up,

  [Description("삼각형 화살표 아래")]
  Arrow_Triangle_Down,

  [Description("타원 화살표 아래")]
  Arrow_Ellipse_Down,

  [Description("타원 화살표 왼쪽")]
  Arrow_Ellipse_Left,

  [Description("타원 화살표 오른쪽")]
  Arrow_Ellipse_Right,

  [Description("타원 화살표 위")]
  Arrow_Ellipse_Up,

  [Description("타원 화살표 아래 A")]
  Arrow_Ellipse_DownA,

  [Description("타원 화살표 왼쪽 A")]
  Arrow_Ellipse_LeftA,

  [Description("타원 화살표 오른쪽 A")]
  Arrow_Ellipse_RightA,

  [Description("타원 화살표 위 A")]
  Arrow_Ellipse_UpA,

  [Description("권한")]
  Authority,

  [Description("뒤로")]
  Back,

  [Description("백업")]
  BackUP,

  [Description("은행")]
  Bank,

  [Description("바코드")]
  BarCode,

  [Description("바코드 A")]
  BarCodeA,

  [Description("바코드 B")]
  BarCodeB,

  [Description("벨 아웃라인")]
  BellOutline,

  [Description("생일")]
  Birthday,

  [Description("브랜드")]
  Brand,

  [Description("브랜드 A")]
  BrandA,

  [Description("브랜드 로고")]
  BrandLogo,

  [Description("방송")]
  Broadcast,

  [Description("예산")]
  BudgetString,

  [Description("버그")]
  Bug,

  [Description("버튼 커서")]
  ButtonCursor,

  [Description("구매")]
  Buy,

  [Description("구매 A")]
  BuyA,

  [Description("구매 B")]
  BuyB,

  [Description("비즈니스 아이템")]
  BusinessItem,

  [Description("비즈니스 타입")]
  BusinessType,

  [Description("캐시")]
  Cache,

  [Description("계산")]
  Calculation,

  [Description("달력")]
  Calender,

  [Description("빈 달력")]
  CalendarBlankOutline,

  [Description("월별 달력")]
  CalendarMonth,

  [Description("취소")]
  Cancel,

  [Description("카드 여러 장")]
  CardMultiple,

  [Description("카드 여러 장 아웃라인")]
  CardMultipleOutline,

  [Description("카드 스페이드 여러 장 아웃라인")]
  Cardsplayingspademultipleoutline,

  [Description("클럽 카드")]
  CardSuitClub,

  [Description("다이아몬드 카드")]
  CardSuitDiamond,

  [Description("하트 카드")]
  CardSuitHeart,

  [Description("스페이드 카드")]
  CardSuitSpade,

  [Description("현금")]
  Cash,

  [Description("현금 A")]
  CashA,

  [Description("현금 100")]
  Cash100,

  [Description("현금 여러 장")]
  CashMulti,

  [Description("카테고리")]
  Category,

  [Description("3D 카테고리")]
  Category3D,

  [Description("차트")]
  Chart,

  [Description("차트 A")]
  ChartA,

  [Description("버블 차트")]
  ChartBubble,

  [Description("파이 차트")]
  ChartPie,

  [Description("파이 차트 A")]
  ChartPieA,

  [Description("채팅")]
  Chat,

  [Description("장바구니")]
  Cart,

  [Description("체크")]
  Check,

  [Description("체크볼드")]
  Checkbold,

  [Description("체크 원형")]
  CheckCircle,

  [Description("체크 데카그램")]
  CheckDecagram,

  [Description("쉐브론 아래")]
  ChevronDown,

  [Description("쉐브론 오른쪽")]
  ChevronRight,

  [Description("도시")]
  City,

  [Description("클라이언트 타원")]
  ClientEllipse,

  [Description("클립보드 체크")]
  ClipboardCheck,

  [Description("클립보드 텍스트 시계")]
  ClipboardTextClock,

  [Description("닫기")]
  Close,

  [Description("클라우드")]
  Cloud,

  [Description("코드")]
  Code,

  [Description("코드 A")]
  CodeA,

  [Description("코드 B")]
  CodeB,

  [Description("코드 C")]
  CodeC,

  [Description("코드 D")]
  CodeD,

  [Description("톱니바퀴")]
  Cog,

  [Description("톱니바퀴 아웃라인")]
  CogOutline,

  [Description("톱니바퀴 새로고침 아웃라인")]
  CogRefreshOutline,

  [Description("댓글")]
  Comment,

  [Description("댓글 아웃라인")]
  CommentOutline,

  [Description("커밋")]
  Commit,

  [Description("회사")]
  Company,

  [Description("콘솔 라인")]
  ConsoleLine,

  [Description("연락처")]
  Contact,

  [Description("콘텐츠 붙여넣기")]
  Contentpaste,

  [Description("계약 타원")]
  ContractEllipse,

  [Description("국가")]
  Country,

  [Description("쿠폰")]
  Coupon,

  [Description("생성")]
  Create,

  [Description("생성 A")]
  CreateA,

  [Description("생성 B")]
  CreateB,

  [Description("생성 C")]
  CreateC,

  [Description("생성 D")]
  CreateD,

  [Description("사각형 생성 플러스")]
  CreateSquarePlus,

  [Description("신용카드")]
  CreditCard,

  [Description("신용카드 A")]
  CreditCardA,

  [Description("신용카드 칩")]
  Creditcardchip,

  [Description("신용카드 칩 아웃라인")]
  CreditcardchipOutline,

  [Description("자르기")]
  Crop,

  [Description("원화")]
  CurrencyWon,

  [Description("커서 포인터")]
  CursorPointer,

  [Description("고객")]
  Customer,

  [Description("고객 A")]
  CustomerA,

  [Description("고객 B")]
  CustomerB,

  [Description("대시보드")]
  Dashboard,

  [Description("대시보드 A")]
  DashboardA,

  [Description("날짜")]
  Date,

  [Description("일")]
  Day,

  [Description("일 타원")]
  DayEllipse,

  [Description("데이터베이스")]
  Database,

  [Description("데이터베이스 플러스")]
  DatabasePlus,

  [Description("삭제")]
  Delete,

  [Description("삭제 A")]
  DeleteA,

  [Description("빈 삭제")]
  DeleteEmpty,

  [Description("배달")]
  Delivery,

  [Description("부서")]
  Department,

  [Description("예금")]
  Deposit,

  [Description("설명")]
  Description,

  [Description("설명 A")]
  DescriptionA,

  [Description("설명 B")]
  DescriptionB,

  [Description("데스크탑 클래식")]
  DesktopClassic,

  [Description("상세")]
  Detail,

  [Description("상세 A")]
  DetailA,

  [Description("상세 B")]
  DetailB,

  [Description("상세 C")]
  DetailC,

  [Description("상세 D")]
  DetailD,

  [Description("상세 E")]
  DetailE,

  [Description("할인")]
  Discount,

  [Description("할인 A")]
  DiscountA,

  [Description("할인 B")]
  DiscountB,

  [Description("도메인")]
  Domain,

  [Description("가로 점")]
  DotsHorizontal,

  [Description("가로 점 원")]
  DotsHorizontalCircle,

  [Description("아래")]
  Down,

  [Description("편집")]
  Edit,

  [Description("편집 A")]
  EditA,

  [Description("편집 B")]
  EditB,

  [Description("이메일")]
  Email,

  [Description("이메일 A")]
  EmailA,

  [Description("이메일 아웃라인")]
  EmailOutline,

  [Description("직원")]
  Employee,

  [Description("직원 A")]
  EmployeeA,

  [Description("환경")]
  Environment,

  [Description("오류")]
  Error,

  [Description("오류 A")]
  ErrorA,

  [Description("엑셀")]
  Excel,

  [Description("교환")]
  Exchange,

  [Description("내보내기")]
  Export,

  [Description("눈 원")]
  EyeCircle,

  [Description("스포이드 변형")]
  EyedropperVariant,

  [Description("페이스북")]
  Facebook,

  [Description("빠름")]
  Fast,

  [Description("즐겨찾기")]
  Favorite,

  [Description("팩스")]
  FAX,

  [Description("파일")]
  File,

  [Description("파일 A")]
  FileA,

  [Description("파일 체크")]
  FileCheck,

  [Description("파일 이미지")]
  FileImage,

  [Description("파일 PDF")]
  FilePdf,

  [Description("파일 Word")]
  FileWord,

  [Description("파일 Zip")]
  FileZip,

  [Description("필터 변형")]
  FilterVariant,

  [Description("금융")]
  Finance,

  [Description("폴더")]
  Folder,

  [Description("폴더 A")]
  FolderA,

  [Description("폴더 열기")]
  FolderOpen,

  [Description("폴더 열기 아웃라인")]
  FolderOpenOutline,

  [Description("폴더 아웃라인")]
  FolderOutline,

  [Description("폴더 테이블")]
  FolderRable,

  [Description("글머리 목록")]
  FormatListBulleted,

  [Description("성별")]
  Gender,

  [Description("유령")]
  Ghost,

  [Description("유령 1")]
  Ghost1,

  [Description("선물")]
  Gift,

  [Description("구글")]
  Google,

  [Description("구글 번역")]
  GoogleTranslate,

  [Description("그리드")]
  Grid,

  [Description("하드디스크")]
  Harddisk,

  [Description("하트")]
  Heart,

  [Description("하트 아웃라인")]
  HeartOutline,

  [Description("Heimdallr 로고")]
  HeimdallrLogo,

  [Description("히스토리")]
  History,

  [Description("히스토리 A")]
  HistoryA,

  [Description("홈")]
  Home,

  [Description("홈 A")]
  HomeA,

  [Description("홈 B")]
  HomeB,

  [Description("홈 원")]
  HomeCircle,

  [Description("홈 원 아웃라인")]
  HomeCircleOutline,

  [Description("홈 아웃라인")]
  HomeOutline,

  [Description("이미지")]
  Image,

  [Description("가져오기")]
  Import,

  [Description("가져오기 A")]
  ImportA,

  [Description("수입")]
  Income,

  [Description("정보")]
  Information,

  [Description("정보 A")]
  InformationA,

  [Description("정보 아웃라인")]
  InformationOutline,

  [Description("인스타그램")]
  Instagram,

  [Description("재고")]
  Inventory,

  [Description("재고 A")]
  InventoryA,

  [Description("송장")]
  Invoice,

  [Description("송장 A")]
  InvoiceA,

  [Description("송장 B")]
  InvoiceB,

  [Description("아이템")]
  Item,

  [Description("아이템 사이트")]
  ItemSite,

  [Description("키")]
  Key,

  [Description("키보드 백스페이스")]
  KeyboardBackspace,

  [Description("라벨")]
  Label,

  [Description("성")]
  LastName,

  [Description("왼쪽")]
  Left,

  [Description("링크")]
  Link,

  [Description("링크 박스")]
  LinkBox,

  [Description("링크드인")]
  Linkedin,

  [Description("링크 변형")]
  LinkVariant,

  [Description("리스트")]
  List,

  [Description("리스트 A")]
  ListA,

  [Description("리스트 B")]
  ListB,

  [Description("리스트 C")]
  ListC,

  [Description("리스트 D")]
  ListD,

  [Description("리스트 E")]
  ListE,

  [Description("리스트 F")]
  ListF,

  [Description("리스트 G")]
  ListG,

  [Description("리스트 H")]
  ListH,

  [Description("리스트 I")]
  ListI,

  [Description("리스트 J")]
  ListJ,

  [Description("잠금")]
  Lock,

  [Description("로그")]
  Log,

  [Description("로그인")]
  Login,

  [Description("로그인 A")]
  LoginA,

  [Description("로그 기록")]
  LogRecord,

  [Description("로그아웃")]
  Logout,

  [Description("로그아웃 A")]
  LogoutA,

  [Description("로그아웃 B")]
  LogoutB,

  [Description("확대")]
  Magnify,

  [Description("관리")]
  Management,

  [Description("관리 A")]
  ManagementA,

  [Description("관리 B")]
  ManagementB,

  [Description("관리 C")]
  ManagementC,

  [Description("매니저")]
  Manager,

  [Description("맵 제작")]
  MapMaker,

  [Description("맵 마커 아웃라인")]
  MapMarkerOutline,

  [Description("최대화")]
  Maximize,

  [Description("메모리")]
  Memory,

  [Description("메뉴")]
  Menu,

  [Description("메뉴 바")]
  MenuBar,

  [Description("메뉴 아래")]
  MenuDown,

  [Description("메뉴 위")]
  MenuUp,

  [Description("마이크로소프트")]
  Microsoft,

  [Description("마이크로소프트 비주얼 스튜디오")]
  MicrosoftVisualStudio,

  [Description("마이크로소프트 윈도우")]
  MicrosoftWindows,

  [Description("최소화")]
  Minimize,

  [Description("마이너스")]
  Minus,

  [Description("휴대폰")]
  MobilePhone,

  [Description("돈")]
  Money,

  [Description("모니터 쉐이머")]
  MonitorShimmer,

  [Description("월")]
  Month,

  [Description("월 요약")]
  MonthSummary,

  [Description("열기 재생 이동")]
  MoveOpenPlay,

  [Description("내비게이션")]
  Navigation,

  [Description("내비게이션 아웃라인")]
  NavigationOutLine,

  [Description("넷플릭스")]
  Netflix,

  [Description("없음")]
  None,

  [Description("알림")]
  Notification,

  [Description("오피스")]
  Office,

  [Description("주문")]
  Order,

  [Description("소유자")]
  Owner,

  [Description("포장")]
  Packaging,

  [Description("포장 A")]
  PackagingA,

  [Description("지급")]
  Paid,

  [Description("팔레트")]
  Palette,

  [Description("비밀번호 키")]
  PasswordKey,

  [Description("비밀번호 잠금")]
  PasswordLock,

  [Description("결제")]
  Payment,

  [Description("PDF")]
  PDF,

  [Description("전화")]
  Phone,

  [Description("사진")]
  Photo,

  [Description("핀")]
  Pin,

  [Description("핀 A")]
  PinA,

  [Description("플러스")]
  Plus,

  [Description("플러스 A")]
  PlusA,

  [Description("플러스 박스")]
  PlusBox,

  [Description("포커 칩")]
  PokerChip,

  [Description("전원")]
  Power,

  [Description("전원 A")]
  PowerA,

  [Description("포인트")]
  Point,

  [Description("위치")]
  Position,

  [Description("위치 A")]
  PositionA,

  [Description("위치 B")]
  PositionB,

  [Description("우편")]
  Postal,

  [Description("우편 A")]
  PostalA,

  [Description("이전")]
  Previous,

  [Description("가격")]
  Price,

  [Description("가격 A")]
  PriceA,

  [Description("가격 타원")]
  PriceEllipse,

  [Description("인쇄")]
  Print,

  [Description("처리")]
  Processing,

  [Description("제품")]
  Product,

  [Description("제품 A")]
  ProductA,

  [Description("제품 B")]
  ProductB,

  [Description("제품 C")]
  ProductC,

  [Description("제품 공장")]
  ProductFactory,

  [Description("제품 생성")]
  ProductCreate,

  [Description("제품 타원")]
  ProductEllipse,

  [Description("제품 반환")]
  ProductReturn,

  [Description("제품 반환 타원")]
  ProductReturnEllipse,

  [Description("제품 텍스트 타원")]
  ProductTextEllipse,

  [Description("제품 경고")]
  ProductWarning,

  [Description("구매")]
  Purchase,

  [Description("구매/매입")]
  Purchase_Buy,

  [Description("QR 코드")]
  QrCode,

  [Description("수량")]
  Quantity,

  [Description("질문")]
  Question,

  [Description("빠른")]
  Quick,

  [Description("읽기")]
  Read,

  [Description("읽기 A")]
  ReadA,

  [Description("텍스트 읽기")]
  ReadText,

  [Description("눈 읽기")]
  ReadEyes,

  [Description("영수증")]
  Receipt,

  [Description("기록")]
  Record,

  [Description("환불")]
  Refund,

  [Description("지역")]
  Region,

  [Description("등록")]
  Registration,

  [Description("요청")]
  Request,

  [Description("보고서")]
  Report,

  [Description("보고서 A")]
  ReportA,

  [Description("크기 조정")]
  Resize,

  [Description("복원")]
  Restore,

  [Description("반환")]
  Return,

  [Description("반품 창고")]
  ReturnedWarehousing,

  [Description("오른쪽")]
  Right,

  [Description("역할")]
  Role,

  [Description("자")]
  Ruler,

  [Description("판매")]
  Sale,

  [Description("판매 A")]
  SaleA,

  [Description("판매 B")]
  SaleB,

  [Description("판매 수익")]
  SaleRevenue,

  [Description("저장")]
  Save,

  [Description("검색")]
  Search,

  [Description("검색 A")]
  SearchA,

  [Description("데이터베이스 검색")]
  SearchDatabase,

  [Description("보안")]
  Security,

  [Description("모두 선택")]
  SelectAll,

  [Description("판매")]
  Sell,

  [Description("판매 A")]
  SellA,

  [Description("설정")]
  Setting,

  [Description("보호 잠금")]
  ShieldLock,

  [Description("발송됨")]
  Shipped,

  [Description("배송")]
  Shipping,

  [Description("다음 건너뛰기")]
  SkipNext,

  [Description("이전 건너뛰기")]
  SkipPrevious,

  [Description("직원")]
  Staff,

  [Description("별")]
  Star,

  [Description("상태")]
  State,

  [Description("상태 A")]
  StateA,

  [Description("상태")]
  Status,

  [Description("거리")]
  Street,

  [Description("재고")]
  Stock,

  [Description("스토어")]
  Store,

  [Description("스토어 A")]
  StoreA,

  [Description("제출")]
  Submit,

  [Description("제출 A")]
  SubmitA,

  [Description("제출 B")]
  SubmitB,

  [Description("성공")]
  Success,

  [Description("성공깃발")]
  Success_Flag,

  [Description("합계")]
  Sum,

  [Description("수평 교환")]
  SwapHorizontal,

  [Description("세금")]
  TAX,

  [Description("세금 계산서")]
  TaxInvoice,

  [Description("세금 계산서 A")]
  TaxInvoiceA,

  [Description("텍스트박스")]
  TextBox,

  [Description("시간표")]
  Timetable,

  [Description("제목")]
  Title,

  [Description("거래")]
  Transaction,

  [Description("휴지통")]
  Trash,

  [Description("휴지통 아웃라인")]
  TrashOutline,

  [Description("트위터")]
  Twitter,

  [Description("유형")]
  Type,

  [Description("단위")]
  Unit,

  [Description("단위 A")]
  UnitA,

  [Description("위")]
  Up,

  [Description("업데이트")]
  Update,

  [Description("업데이트 A")]
  UpdateA,

  [Description("업데이트 B")]
  UpdateB,

  [Description("사용자")]
  User,

  [Description("공급업체")]
  Vendor,

  [Description("보기 - 아젠다")]
  ViewAgenda,

  [Description("보기 - 컬럼")]
  ViewColumn,

  [Description("보기 - 컴팩트")]
  ViewCompact,

  [Description("보기 - 그리드")]
  ViewGrid,

  [Description("뷰")]
  Views,

  [Description("창고")]
  Warehouse,

  [Description("야간 날씨")]
  WeatherNight,

  [Description("경고")]
  Warning,

  [Description("웹")]
  Web,

  [Description("창 최대화")]
  WindowMaximize,

  [Description("창 최소화")]
  WindowMinimize,

  [Description("화이트 밸런스 - 맑음")]
  WhiteBalanceSunny,

  [Description("년")]
  Year,

  [Description("유튜브")]
  YouTube
}



