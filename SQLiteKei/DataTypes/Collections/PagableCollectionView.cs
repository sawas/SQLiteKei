using System.Collections;
using System.ComponentModel;
using System.Windows.Data;

namespace SQLiteKei.DataTypes.Collections
{
    /// <summary>
    /// A collection view that allows pagination on its values.
    /// </summary>
    public class PagableCollectionView : ListCollectionView
    {
        private readonly IList innerList;

        private int itemsPerPage;
        public int ItemsPerPage
        {
            get { return itemsPerPage; }
            set { itemsPerPage = value >= 0 ? value : 1; Refresh(); }
        }

        public int PageCount
        {
            get
            {
                return (innerList.Count + ItemsPerPage - 1) / ItemsPerPage;
            }
        }

        public override int Count
        {
            get
            {
                if (innerList.Count == 0)
                    return 0;

                if (CurrentPage < PageCount)
                {
                    return ItemsPerPage;
                }
                else
                {
                    var remainingItems = innerList.Count % ItemsPerPage;
                    if (remainingItems == 0)
                    {
                        return ItemsPerPage;
                    }
                    else
                    {
                        return remainingItems;
                    }
                }
            }
        }

        private int currentPage;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
            }
        }

        private int StartIndex
        {
            get { return (currentPage - 1) * ItemsPerPage; }
        }

        public override object GetItemAt(int index)
        {
            var offset = index % (ItemsPerPage);
            return innerList[StartIndex + offset];
        }

        private int EndIndex
        {
            get
            {
                var lastIndex = currentPage * ItemsPerPage - 1;
                return (lastIndex > innerList.Count ? innerList.Count : lastIndex);
            }
        }

        public PagableCollectionView(IList innerList, int itemsPerPage = 30)
            : base(innerList)
        {
            CurrentPage = 1;
            this.innerList = innerList;
            ItemsPerPage = itemsPerPage;
        }

        public void MoveToNextPage()
        {
            if (currentPage < PageCount)
            {
                CurrentPage += 1;
                Refresh();
            }
        }

        public void MoveToPreviousPage()
        {
            if (currentPage > 1)
            {
                CurrentPage -= 1;
                Refresh();
            }
        }

        public void MoveToFirstPage()
        {
            if (currentPage != 1)
            {
                CurrentPage = 1;
                Refresh();
            }
        }

        public void MoveToLastPage()
        {
            if (currentPage != PageCount)
            {
                CurrentPage = PageCount;
                Refresh();
            }
        }

        protected override void RefreshOverride()
        {
            base.RefreshOverride();
        }
    }
}
