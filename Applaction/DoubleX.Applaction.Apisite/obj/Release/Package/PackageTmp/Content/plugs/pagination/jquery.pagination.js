/**
 * This jQuery plugin displays pagination links inside the selected elements.
 * 
 * This plugin needs at least jQuery 1.4.2
 *
 * @author Gabriel Birke (birke *at* d-scribe *dot* de)
 * @version 2.2
 * @param {int} dataCount Number of entries to paginate
 * @param {Object} opts Several options (see README for documentation)
 * @return {Object} jQuery Object
 */
 (function($){
	/**
	 * @class Class for calculating pagination values
	 */
	$.PaginationCalculator = function(dataCount, opts) {
		this.dataCount = dataCount;
		this.opts = opts;
	};
	
	$.extend($.PaginationCalculator.prototype, {
		/**
		 * Calculate the maximum number of pages
		 * @method
		 * @returns {Number}
		 */
		numPages:function() {
			return Math.ceil(this.dataCount/this.opts.pageSize);
		},
		/**
		 * Calculate start and end point of pagination links depending on 
		 * curPageIndex and num_display_entries.
		 * @returns {Array}
		 */
		getInterval:function(curPageIndex)  {
			var ne_half = Math.floor(this.opts.num_display_entries/2);
			var np = this.numPages();
			var upper_limit = np - this.opts.num_display_entries;
			var start = curPageIndex > ne_half ? Math.max( Math.min(curPageIndex - ne_half, upper_limit), 0 ) : 0;
			var end = curPageIndex > ne_half?Math.min(curPageIndex+ne_half + (this.opts.num_display_entries % 2), np):Math.min(this.opts.num_display_entries, np);
			return {start:start, end:end};
		}
	});
	
	// Initialize jQuery object container for pagination renderers
	$.PaginationRenderers = {};
	
	/**
	 * @class Default renderer for rendering pagination links
	 */
	$.PaginationRenderers.defaultRenderer = function(dataCount, opts) {
		this.dataCount = dataCount;
		this.opts = opts;
		this.pc = new $.PaginationCalculator(dataCount, opts);
	};
	$.extend($.PaginationRenderers.defaultRenderer.prototype, {
		/**
		 * Helper function for generating a single link (or a span tag if it's the current page)
		 * @param {Number} page_id The page id for the new item
		 * @param {Number} curPageIndex 
		 * @param {Object} appendopts Options for the new item: text and classes
		 * @returns {jQuery} jQuery object containing the link
		 */
		createLink:function(page_id, curPageIndex, appendopts){
			var lnk, np = this.pc.numPages();
			page_id = page_id<0?0:(page_id<np?page_id:np-1); // Normalize page id to sane value
			appendopts = $.extend({text:page_id+1, classes:""}, appendopts||{});
			if(page_id == curPageIndex){
				lnk = $("<span class='current'>" + appendopts.text + "</span>");
			}
			else
			{
				lnk = $("<a>" + appendopts.text + "</a>")
					.attr('href', this.opts.link_to.replace(/__id__/,page_id));
			}
			if(appendopts.classes){ lnk.addClass(appendopts.classes); }
			if(appendopts.rel){ lnk.attr('rel', appendopts.rel); }
			lnk.data('page_id', page_id);
			return lnk;
		},
		// Generate a range of numeric links 
		appendRange:function(container, curPageIndex, start, end, opts) {
			var i;
			for(i=start; i<end; i++) {
				this.createLink(i, curPageIndex, opts).appendTo(container);
			}
		},
		getLinks:function(curPageIndex, eventHandler) {
			var begin, end,
				interval = this.pc.getInterval(curPageIndex),
				np = this.pc.numPages(),
				fragment = $("<div class='pagination'></div>");
			
			// Generate "Previous"-Link
			if(this.opts.prevText && (curPageIndex > 0 || this.opts.prevShow)){
				fragment.append(this.createLink(curPageIndex-1, curPageIndex, {text:this.opts.prevText, classes:"prev",rel:"prev"}));
			}
			// Generate starting points
			if (interval.start > 0 && this.opts.pageItemNum > 0)
			{
				end = Math.min(this.opts.pageItemNum, interval.start);
				this.appendRange(fragment, curPageIndex, 0, end, {classes:'sp'});
				if(this.opts.pageItemNum < interval.start && this.opts.ellipseText)
				{
					$("<span>"+this.opts.ellipseText+"</span>").appendTo(fragment);
				}
			}
			// Generate interval links
			this.appendRange(fragment, curPageIndex, interval.start, interval.end);
			// Generate ending points
			if (interval.end < np && this.opts.pageItemNum > 0)
			{
				if(np-this.opts.pageItemNum > interval.end && this.opts.ellipseText)
				{
					$("<span>"+this.opts.ellipseText+"</span>").appendTo(fragment);
				}
				begin = Math.max(np-this.opts.pageItemNum, interval.end);
				this.appendRange(fragment, curPageIndex, begin, np, {classes:'ep'});
				
			}
			// Generate "Next"-Link
			if(this.opts.nextText && (curPageIndex < np-1 || this.opts.nextShow)){
				fragment.append(this.createLink(curPageIndex+1, curPageIndex, {text:this.opts.nextText, classes:"next",rel:"next"}));
			}
			$('a', fragment).click(eventHandler);
			return fragment;
		}
	});
	
	// Extend jQuery
	$.fn.pagination = function(dataCount, opts){
		
		// Initialize options with default values
		opts = $.extend({
		    pageSize: 10,                            //每页显示的条目数
			num_display_entries: 11,                 //连续分页主体部分显示的分页条目数
			curPageIndex:0,                          //当前页
			pageItemNum: 0,                         //两侧显示的首尾分页的条目数
			link_to: "javascript:;",                //分页的链接
			prevText: "Prev",                       //“前一页”分页按钮上显示的文字      
			nextText: "Next",                       //“下一页”分页按钮上显示的文字
			ellipseText: "...",                     //省略的页数用什么文字表示
			prevShow:true,
			nextShow:true,
			renderer:"defaultRenderer",
			show_if_single_page:false,
			load_first_page:true,
			callback:function(){return false;}
		},opts||{});
		
		var containers = this,
			renderer, links, curPageIndex;
		
		/**
		 * This is the event handling function for the pagination links. 
		 * @param {int} page_id The new page number
		 */
		function paginationClickHandler(evt){
			var links, 
				new_curPageIndex = $(evt.target).data('page_id'),
				continuePropagation = selectPage(new_curPageIndex);
			if (!continuePropagation) {
				evt.stopPropagation();
			}
			return continuePropagation;
		}
		
		/**
		 * This is a utility function for the internal event handlers. 
		 * It sets the new current page on the pagination container objects, 
		 * generates a new HTMl fragment for the pagination links and calls
		 * the callback function.
		 */
		function selectPage(new_curPageIndex) {
			// update the link display of a all containers
			containers.data('curPageIndex', new_curPageIndex);
			links = renderer.getLinks(new_curPageIndex, paginationClickHandler);
			containers.empty();
			links.appendTo(containers);
			// call the callback and propagate the event if it does not return false
			var continuePropagation = opts.callback(new_curPageIndex, containers);
			return continuePropagation;
		}
		
		// -----------------------------------
		// Initialize containers
		// -----------------------------------
		curPageIndex = parseInt(opts.curPageIndex, 10);
		containers.data('curPageIndex', curPageIndex);
		// Create a sane value for dataCount and pageSize
		dataCount = (!dataCount || dataCount < 0)?1:dataCount;
		opts.pageSize = (!opts.pageSize || opts.pageSize < 0)?1:opts.pageSize;
		
		if(!$.PaginationRenderers[opts.renderer])
		{
			throw new ReferenceError("Pagination renderer '" + opts.renderer + "' was not found in jQuery.PaginationRenderers object.");
		}
		renderer = new $.PaginationRenderers[opts.renderer](dataCount, opts);
		
		// Attach control events to the DOM elements
		var pc = new $.PaginationCalculator(dataCount, opts);
		var np = pc.numPages();
		containers.off('setPage').on('setPage', {numPages:np}, function(evt, page_id) { 
				if(page_id >= 0 && page_id < evt.data.numPages) {
					selectPage(page_id); return false;
				}
		});
		containers.off('prevPage').on('prevPage', function(evt){
				var curPageIndex = $(this).data('curPageIndex');
				if (curPageIndex > 0) {
					selectPage(curPageIndex - 1);
				}
				return false;
		});
		containers.off('nextPage').on('nextPage', {numPages:np}, function(evt){
				var curPageIndex = $(this).data('curPageIndex');
				if(curPageIndex < evt.data.numPages - 1) {
					selectPage(curPageIndex + 1);
				}
				return false;
		});
		containers.off('currentPage').on('currentPage', function(){
				var curPageIndex = $(this).data('curPageIndex');
				selectPage(curPageIndex);
				return false;
		});
		
		// When all initialisation is done, draw the links
		links = renderer.getLinks(curPageIndex, paginationClickHandler);
		containers.empty();
		if(np > 1 || opts.show_if_single_page) {
			links.appendTo(containers);
		}
		// call callback function
		if(opts.load_first_page) {
			opts.callback(curPageIndex, containers);
		}
	}; // End of $.fn.pagination block
	
})(jQuery);
